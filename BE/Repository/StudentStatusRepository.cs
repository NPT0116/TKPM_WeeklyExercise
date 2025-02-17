using BE.Data;
using BE.Interface;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository
{
    public class StudentStatusRepository : IStudentStatusRepository
    {
        private readonly AppDbContext _context;

        public StudentStatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StudentStatus>> GetAllAsync()
        {
            return await _context.StudentStatuses.ToListAsync();
        }

        public async Task<StudentStatus> GetByIdAsync(int id)
        {
            return await _context.StudentStatuses.FindAsync(id);
        }

        public async Task<StudentStatus> CreateAsync(StudentStatus status)
        {
            _context.StudentStatuses.Add(status);
            await _context.SaveChangesAsync();
            return status;
        }

        public async Task UpdateAsync(StudentStatus status)
        {
            _context.Entry(status).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var status = await _context.StudentStatuses.FindAsync(id);
            if (status != null)
            {
                _context.StudentStatuses.Remove(status);
                await _context.SaveChangesAsync();
            }
        }
    }
} 