using System;
using BE.Dto;
using BE.Exceptions.ApplicationProgram;
using BE.Exceptions.Faculty;
using BE.Exceptions.Student;
using BE.Exceptions.StudentStatus;
using BE.Interface;
using BE.Models;

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
    public StudentService(IStudentRepository studentRepository, IValidateStudentEmail validateStudentEmail, IValidateStudentPhone validateStudentPhone, IFacultyRepository facultyRepository, IApplicationProgramRepository applicationProgramRepository, IStudentStatusRepository studentStatusRepository, IStudentStatusTransitionService studentStatusTransitionService)
    {
        _studentRepository = studentRepository;
        _validateStudentEmail = validateStudentEmail;
        _validateStudentPhone = validateStudentPhone;
        _facultyRepository = facultyRepository;
        _applicationProgramRepository = applicationProgramRepository;
        _studentStatusRepository = studentStatusRepository;
        _studentStatusTransitionService = studentStatusTransitionService;
    }
    public async Task<StudentCreateDto> AddStudentServiceAsync(StudentCreateDto student)
    {
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
        var studentStatus = await _studentStatusRepository.GetByIdAsync(student.StatusId);
        if (studentStatus == null)
        {
            throw new StudentStatusNotFound(student.StatusId);
        }
        await _studentRepository.CreateAsync(student);
        return student;
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
