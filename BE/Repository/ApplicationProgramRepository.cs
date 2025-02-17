using BE.Data;
using BE.Interface;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class ApplicationProgramRepository : IApplicationProgramRepository
    {
        private readonly AppDbContext _context;

        public ApplicationProgramRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationProgram>> GetAllAsync()
        {
            return await _context.ApplicationPrograms.ToListAsync();
        }

        public async Task<ApplicationProgram> GetByIdAsync(int id)
        {
            return await _context.ApplicationPrograms.FindAsync(id);
        }

        public async Task<ApplicationProgram> CreateAsync(ApplicationProgram program)
        {
            _context.ApplicationPrograms.Add(program);
            await _context.SaveChangesAsync();
            return program;
        }

        public async Task UpdateAsync(ApplicationProgram program)
        {
            _context.Entry(program).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var program = await _context.ApplicationPrograms.FindAsync(id);
            if (program != null)
            {
                _context.ApplicationPrograms.Remove(program);
                await _context.SaveChangesAsync();
            }
        }
    }
} 