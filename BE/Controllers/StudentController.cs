using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dto;
using BE.Interface;
using BE.Models;
using BE.Services;
using BE.Utils; // Ensure you include the namespace for Response
using Microsoft.AspNetCore.Mvc;

namespace BE.Controller
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentExportService _exportService;
        private readonly IStudentImportService _importService;

        public StudentController(
            IStudentService studentService,
            IStudentRepository studentRepo,
            IStudentExportService exportService,
            IStudentImportService importService)
        {
            _studentService = studentService;
            _studentRepo = studentRepo;
            _exportService = exportService;
            _importService = importService;
        }

        [HttpGet]
        public async Task<ActionResult<Response<List<StudentDto>>>> GetAllStudents()
        {
            var students = await _studentRepo.GetAllAsync();
            return Ok(new Response<List<StudentDto>>(students));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<StudentDto>>> GetStudent(string id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null)
            {
                return NotFound(new Response<StudentDto>
                {
                    Succeeded = false,
                    Message = "Student not found.",
                    Errors = new[] { "Student not found." }
                });
            }
            return Ok(new Response<StudentDto>(student));
        }

        [HttpPost]
        public async Task<ActionResult<Response<StudentCreateDto>>> AddStudent(StudentCreateDto student)
        {
            try
            {
                var newStudent = await _studentService.AddStudentServiceAsync(student);
                return CreatedAtAction(nameof(GetStudent), new { id = newStudent.StudentId }, new Response<StudentCreateDto>(newStudent));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<StudentCreateDto>
                {
                    Succeeded = false,
                    Message = ex.Message,
                    Errors = new[] { ex.Message }
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Response<StudentUpdateDto>>> UpdateStudent(string id, [FromBody] StudentUpdateDto student)
        {
            if (id != student.StudentId)
            {
                return BadRequest(new Response<StudentUpdateDto>
                {
                    Succeeded = false,
                    Message = "Student ID mismatch.",
                    Errors = new[] { "Student ID mismatch." }
                });
            }

            try
            {
                var updatedStudent = await _studentService.UpdateStudentServiceAsync(student);
                return Ok(new Response<StudentUpdateDto>(updatedStudent, "Student updated successfully.", true));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<StudentUpdateDto>
                {
                    Succeeded = false,
                    Message = ex.Message,
                    Errors = new[] { ex.Message }
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteStudent(string id)
        {
            await _studentService.DeleteStudentServiceAsync(id);
            return Ok(new Response<string>(null, "Student deleted successfully.", true));
        }

        [HttpGet("search")]
        public async Task<ActionResult<Response<List<StudentDto>>>> SearchStudents(
            [FromQuery] int? facultyId,
            [FromQuery] string name)
        {
            var students = await _studentRepo.SearchAsync(facultyId, name);
            return Ok(new Response<List<StudentDto>>(students));
        }

        [HttpGet("faculty/{facultyId}")]
        public async Task<ActionResult<Response<List<StudentDto>>>> GetStudentsByFacultyId(int facultyId)
        {
            var students = await _studentRepo.GetStudentsByFacultyIdAsync(facultyId);
            return Ok(new Response<List<StudentDto>>(students));
        }

        [HttpGet("export/excel")]
        public async Task<IActionResult> ExportStudentsToExcel()
        {
            var content = await _exportService.ExportStudentsToExcelAsync();
            return File(content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "students.xlsx");
        }

        [HttpGet("export/json")]
        public async Task<IActionResult> ExportStudentsToJson()
        {
            var content = await _exportService.ExportStudentsToJsonAsync();
            return File(content, "application/json", "students.json");
        }

        [HttpPost("import/excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ImportStudentsFromExcel([FromForm] FileUploadDto fileUpload)
        {
            if (fileUpload == null || fileUpload.File == null || fileUpload.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            await _importService.ImportStudentsFromExcelAsync(fileUpload.File);
            return Ok(new Response<string>(null, "Import successful", true));
        }
    }
}
