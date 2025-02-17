using System;
using BE.Models;

namespace BE.Interface;

    public interface IApplicationProgramRepository
    {
        Task<IEnumerable<ApplicationProgram>> GetAllAsync();
        Task<ApplicationProgram> GetByIdAsync(int id);
        Task<ApplicationProgram> CreateAsync(ApplicationProgram program);
        Task UpdateAsync(ApplicationProgram program);
        Task DeleteAsync(int id);
    }
