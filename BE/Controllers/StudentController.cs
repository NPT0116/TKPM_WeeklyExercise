using System;
using BE.Dto;
using BE.Interface;
using BE.Models;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controller;


  [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepo;

        public StudentController(IStudentRepository studentRepo)
        {
            _studentRepo = studentRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
        {
            return await _studentRepo.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(string id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) return NotFound();
            return student;
        }

        [HttpPost]
        public async Task<ActionResult> AddStudent(StudentCreateDto student)
        {
            var existingStudent = await _studentRepo.GetByIdAsync(student.StudentId);
            if (existingStudent != null)
            {
                return BadRequest("Student with this ID already exists");
            }
            await _studentRepo.CreateAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateStudent(string id, StudentUpdateDto student)
        {
            if (id != student.StudentId) return BadRequest();
            var existingStudent = await _studentRepo.GetByIdAsync(id);
            if (existingStudent == null) return NotFound();
            await _studentRepo.UpdateAsync(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteStudent(string id)
        {
            await _studentRepo.DeleteAsync(id);
            return NoContent();
        }


    }
