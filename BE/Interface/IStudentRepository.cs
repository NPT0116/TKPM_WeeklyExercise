using System;
using BE.Dto;
using BE.Models;

namespace BE.Interface;

public interface IStudentRepository
{
        public Task<List<StudentDto>> GetAllAsync();
        public Task<StudentDto> GetByIdAsync(string id);
        public Task CreateAsync(StudentCreateDto student);
        public Task UpdateAsync(StudentUpdateDto student);
        public Task<bool> DeleteAsync(string id);

        public Task<List<StudentDto>> GetStudentsByFacultyIdAsync(int facultyId);

        public Task<List<StudentDto>> GetStudentsByProgramIdAsync(int programId);
        public Task<List<StudentDto>> GetStudentsByStatusIdAsync(int statusId);
        public  Task<List<StudentDto>> SearchAsync(int? facultyId, string name);
}
