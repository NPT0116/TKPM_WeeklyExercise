using System;
using BE.Models;

namespace BE.Interface;

    public interface IStudentStatusRepository
    {
        Task<IEnumerable<StudentStatus>> GetAllAsync();
        Task<StudentStatus> GetByIdAsync(int id);
        Task<StudentStatus> CreateAsync(StudentStatus status);
        Task UpdateAsync(StudentStatus status);
        Task DeleteAsync(int id);
    }