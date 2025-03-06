using System;
using BE.Dto;
using BE.Models;

namespace BE.Interface;

public interface IStudentService
{
    Task<StudentCreateDto> AddStudentServiceAsync(StudentCreateDto student);
    Task<StudentUpdateDto> UpdateStudentServiceAsync(StudentUpdateDto student);

    Task DeleteStudentServiceAsync(string id);
    
}
