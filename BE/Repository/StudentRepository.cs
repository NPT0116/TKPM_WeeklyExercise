using System;
using BE.Data;
using BE.Dto;
using BE.Interface;
using BE.Models;
using Microsoft.EntityFrameworkCore;

namespace BE.Repository;

public class StudentRepository : IStudentRepository
{
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StudentDto>> GetAllAsync()
        {
            var students = await _context.Students.Include(s => s.Faculty).Include(s => s.Program).Include(s => s.Status).ToListAsync();
            
            return students.Select(student => new StudentDto
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,    
                Faculty = new facultyDto
                (
                    student.Faculty.FacultyId,
                     student.Faculty.Name
                ),
                Course = student.Course,
                Program = new applicationProgramDto
                (

                    student.Program.ProgramId,
                   student.Program.Name
                ),
                Address = student.Address,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Status = new studentStatusDto
                (
                    student.Status.StatusId,
                    student.Status.Name
                )
            }).ToList();
        }

        public async Task<StudentDto> GetByIdAsync(string id)
        {
            var student = await _context.Students.Include(s => s.Faculty).Include(s => s.Program).Include(s => s.Status).FirstOrDefaultAsync(s => s.StudentId == id);
            if (student == null)
            {
                return null;
            }
            return new StudentDto
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                Faculty = new facultyDto
                (
                    student.Faculty.FacultyId,
                    student.Faculty.Name
                ),
                Course = student.Course,
                Program = new applicationProgramDto
                (
                    student.Program.ProgramId,
                    student.Program.Name    
                ),
                Address = student.Address,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                Status = new studentStatusDto
                (
                    student.Status.StatusId,
                    student.Status.Name
                )
            };
        }

        public async Task CreateAsync(StudentCreateDto student)
        {
            var studentEntity = new Student
            {
                StudentId = student.StudentId,
                FullName = student.FullName,
                DateOfBirth = student.DateOfBirth,
                Gender = student.Gender,
                FacultyId = student.FacultyId,
                Course = student.Course,
                ProgramId = student.ProgramId,
                Address = student.Address,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                StatusId = student.StatusId
            };
            

            await _context.Students.AddAsync(studentEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(StudentUpdateDto student)
        {
            var studentEntity = await _context.Students.FindAsync(student.StudentId);
            if (studentEntity == null)
            {
                throw new KeyNotFoundException($"Student with ID {student.StudentId} not found");
            }

            studentEntity.FullName = student.FullName;
            studentEntity.DateOfBirth = student.DateOfBirth.ToUniversalTime();
            studentEntity.Gender = student.Gender;
            studentEntity.FacultyId = student.FacultyId;
            studentEntity.Course = student.Course;
            studentEntity.ProgramId = student.ProgramId;
            studentEntity.Address = student.Address;
            studentEntity.Email = student.Email;
            studentEntity.PhoneNumber = student.PhoneNumber;
            studentEntity.StatusId = student.StatusId;

            _context.Students.Update(studentEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

}
