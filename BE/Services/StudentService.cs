using System;
using System.Threading.Tasks;
using BE.Config;
using BE.Dto;
using BE.Exceptions.ApplicationProgram;
using BE.Exceptions.Faculty;
using BE.Exceptions.Student;
using BE.Exceptions.StudentStatus;
using BE.Interface;
using BE.Models;
using Microsoft.Extensions.Options;

namespace BE.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IValidateStudentEmail _validateStudentEmail;
        private readonly IValidateStudentPhone _validateStudentPhone;
        private readonly IFacultyRepository _facultyRepository;
        private readonly IApplicationProgramRepository _applicationProgramRepository;
        private readonly IStudentStatusRepository _studentStatusRepository;
        private readonly IStudentStatusTransitionService _studentStatusTransitionService;
        private readonly IBusinessRulesService _businessRulesService;
        private readonly StudentDeletionSetting _deletionTimeSetting;
        private readonly IEmailSender _emailSender;

        public StudentService(
            IStudentRepository studentRepository,
            IValidateStudentEmail validateStudentEmail,
            IValidateStudentPhone validateStudentPhone,
            IFacultyRepository facultyRepository,
            IApplicationProgramRepository applicationProgramRepository,
            IStudentStatusRepository studentStatusRepository,
            IStudentStatusTransitionService studentStatusTransitionService,
            IBusinessRulesService businessRulesService,
            IOptions<StudentDeletionSetting> deletionTimeSetting,
            IEmailSender emailSender)
        {
            _studentRepository = studentRepository;
            _validateStudentEmail = validateStudentEmail;
            _validateStudentPhone = validateStudentPhone;
            _facultyRepository = facultyRepository;
            _applicationProgramRepository = applicationProgramRepository;
            _studentStatusRepository = studentStatusRepository;
            _studentStatusTransitionService = studentStatusTransitionService;
            _businessRulesService = businessRulesService;
            _deletionTimeSetting = deletionTimeSetting.Value;
            _emailSender = emailSender;
        }

        public async Task<StudentCreateDto> AddStudentServiceAsync(StudentCreateDto student)
        {
            var existingStudent = await _studentRepository.GetByIdAsync(student.StudentId);
            if (existingStudent != null)
            {
                throw new StudentAlreadyExists(student.StudentId);
            }
            if (_businessRulesService.GetSettings().EnableEmailValidation &&
                !_validateStudentEmail.IsValidEmail(student.Email))
            {
                throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
            }
            if (_businessRulesService.GetSettings().EnablePhoneValidation &&
                !_validateStudentPhone.IsValidPhone(student.PhoneNumber))
            {
                throw new StudentPhoneFormatError(_validateStudentPhone.GetAllowedPattern());
            }
            var faculty = await _facultyRepository.GetByIdAsync(student.FacultyId);
            if (faculty == null)
            {
                throw new FacultyNotFound(student.FacultyId);
            }
            var applicationProgram = await _applicationProgramRepository.GetByIdAsync(student.ProgramId);
            if (applicationProgram == null)
            {
                throw new ApplicationProgramNotFound(student.ProgramId);
            }

            await _studentRepository.CreateAsync(student);
            return student;
        }

        public async Task DeleteStudentServiceAsync(string id)
        {
            StudentDto existingStudent = await _studentRepository.GetByIdAsync(id);
            if (existingStudent == null)
            {
                throw new StudentNotFound(id);
            }

            // If deletion rule is enabled in business rules, enforce time limit.
            if (_businessRulesService.GetSettings().EnableStudentDeletion)
            {
                var deletionThreshold = DateTime.Now.AddMinutes(-_deletionTimeSetting.AllowedDeletionMinutes);
                if (existingStudent.CreatedAt < deletionThreshold)
                {
                    throw new StudentCantDeleteException($"Student can't be deleted after {_deletionTimeSetting.AllowedDeletionMinutes} minutes");
                }
            }

            await _studentRepository.DeleteAsync(id);
        }

        public async Task<StudentUpdateDto> UpdateStudentServiceAsync(StudentUpdateDto student)
        {
            if (_businessRulesService.GetSettings().EnableEmailValidation &&
                !_validateStudentEmail.IsValidEmail(student.Email))
            {
                throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
            }
            if (_businessRulesService.GetSettings().EnablePhoneValidation &&
                !_validateStudentPhone.IsValidPhone(student.PhoneNumber))
            {
                throw new StudentPhoneFormatError(_validateStudentPhone.GetAllowedPattern());
            }
            var existingStudent = await _studentRepository.GetByIdAsync(student.StudentId);
            if (existingStudent == null)
            {
                throw new StudentNotFound(student.StudentId);
            }
            var faculty = await _facultyRepository.GetByIdAsync(student.FacultyId);
            if (faculty == null)
            {
                throw new FacultyNotFound(student.FacultyId);
            }
            var applicationProgram = await _applicationProgramRepository.GetByIdAsync(student.ProgramId);
            if (applicationProgram == null)
            {
                throw new ApplicationProgramNotFound(student.ProgramId);
            }
            var studentStatus = await _studentStatusRepository.GetByIdAsync(student.StatusId);
            if (studentStatus == null)
            {
                throw new StudentStatusNotFound(student.StatusId);
            }
            if (_businessRulesService.GetSettings().EnableStudentStatusTransition)
            {
                var isValidTransition = _studentStatusTransitionService.IsValidTransition(existingStudent.Status.Id, student.StatusId);
                if (!isValidTransition)
                {
                    throw new StudentStatusTransisentError("Invalid student status transition");
                }
            }

            await _studentRepository.UpdateAsync(student);

            // After a successful update, send a notification email to the student.
            if (!string.IsNullOrEmpty(student.Email))
            {
                string subject = "Thông báo cập nhật thông tin sinh viên";
                string body = $"Chào {existingStudent.FullName},\n\n" +
                              "Thông tin của bạn đã được cập nhật thành công.\n" +
                              $"Trạng thái mới: {student.StatusId}\n\n" +
                              "Trân trọng,\n" +
                              "Phòng Đào Tạo";
                await _emailSender.SendEmailAsync(student.Email, subject, body);
            }

            return student;
        }
    }
}
