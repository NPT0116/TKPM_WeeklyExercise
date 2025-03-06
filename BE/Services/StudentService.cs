using System;
using BE.Config;
using BE.Dto;
using BE.Exceptions.ApplicationProgram;
using BE.Exceptions.Faculty;
using BE.Exceptions.Student;
using BE.Exceptions.StudentStatus;
using BE.Interface;
using BE.Models;
using Microsoft.Extensions.Options;

namespace BE.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IValidateStudentEmail _validateStudentEmail;
    private readonly IValidateStudentPhone _validateStudentPhone;
    private readonly IFacultyRepository _facultyRepository;
    private readonly IApplicationProgramRepository _applicationProgramRepository;
    private readonly IStudentStatusRepository _studentStatusRepository;
    private readonly IStudentStatusTransitionService _studentStatusTransitionService;
    private readonly StudentDeletionSetting _studentDeletionSetting;
    public StudentService(IOptions<StudentDeletionSetting> options,IStudentRepository studentRepository, IValidateStudentEmail validateStudentEmail, IValidateStudentPhone validateStudentPhone, IFacultyRepository facultyRepository, IApplicationProgramRepository applicationProgramRepository, IStudentStatusRepository studentStatusRepository, IStudentStatusTransitionService studentStatusTransitionService)
    {
        _studentRepository = studentRepository;
        _validateStudentEmail = validateStudentEmail;
        _validateStudentPhone = validateStudentPhone;
        _facultyRepository = facultyRepository;
        _applicationProgramRepository = applicationProgramRepository;
        _studentStatusRepository = studentStatusRepository;
        _studentStatusTransitionService = studentStatusTransitionService;
        _studentDeletionSetting = options.Value;
    }
    public async Task<StudentCreateDto> AddStudentServiceAsync(StudentCreateDto student)
    {
        var existingStudent = await _studentRepository.GetByIdAsync(student.StudentId);
        if (existingStudent != null)
        {
            throw new StudentAlreadyExists(student.StudentId);
        }
        if (!_validateStudentEmail.IsValidEmail(student.Email))
        {
            throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
        }
        if (!_validateStudentPhone.IsValidPhone(student.PhoneNumber))
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
        StudentDto existingStudent =  await _studentRepository.GetByIdAsync(id);
        if (existingStudent == null)
        {
            throw new StudentNotFound(id);
        }
        var deletionTime = DateTime.Now.AddMinutes(-_studentDeletionSetting.AllowedDeletionMinutes);
        if (existingStudent.CreatedAt < deletionTime)
        {
            throw new StudentCantDeleteException($"Student can't be deleted after {_studentDeletionSetting.AllowedDeletionMinutes} minutes");
        }
        await _studentRepository.DeleteAsync(id);
    }

    public async Task<StudentUpdateDto> UpdateStudentServiceAsync(StudentUpdateDto student)
    {
        if (!_validateStudentEmail.IsValidEmail(student.Email))
        {
            throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
        }
        if (!_validateStudentPhone.IsValidPhone(student.PhoneNumber))
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
        var isValidTransition = _studentStatusTransitionService.IsValidTransition(existingStudent.Status.Id, student.StatusId);
        if (!isValidTransition)
        {
            throw new StudentStatusTransisentError("Invalid student status transition");
        }

        await _studentRepository.UpdateAsync(student);
        return student;
    }
    
}
