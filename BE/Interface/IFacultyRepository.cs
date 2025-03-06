using System;
using BE.Models;

namespace BE.Interface;

    public interface IFacultyRepository
    {
        Task<IEnumerable<Faculty>> GetAllAsync();
        Task<Faculty> GetByIdAsync(int id);
        Task<Faculty> CreateAsync(Faculty faculty);
        Task UpdateAsync(Faculty faculty);
        Task DeleteAsync(int id);

    }