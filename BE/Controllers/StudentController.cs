using System.Collections.Generic;
using System.Threading.Tasks;
using BE.Dto;
using BE.Exceptions.Student;
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
        private readonly IStudentRepository _studentRepo;
        private readonly IStudentExportService _exportService;
        private readonly IStudentImportService _importService;
        private readonly IValidateStudentEmail _validateStudentEmail;
        public StudentController(IStudentRepository studentRepo, IStudentExportService exportService, IStudentImportService importService, IValidateStudentEmail validateStudentEmail)
        {
            _studentRepo = studentRepo;
            _exportService = exportService;
            _importService = importService;
            _validateStudentEmail = validateStudentEmail;
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
            var existingStudent = await _studentRepo.GetByIdAsync(student.StudentId);
            if (existingStudent != null)
            {
                return BadRequest(new Response<StudentCreateDto>
                {
                    Succeeded = false,
                    Message = "Student with this ID already exists.",
                    Errors = new[] { "Student with this ID already exists." }
                });
            }
            if (!_validateStudentEmail.IsValidEmail(student.Email))
            {
                throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
            }
            await _studentRepo.CreateAsync(student);
            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, new Response<StudentCreateDto>(student));
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
            if (!_validateStudentEmail.IsValidEmail(student.Email))
            {
                throw new StudentEmailFormatError(_validateStudentEmail.GetAllowedDomain());
            }
            var existingStudent = await _studentRepo.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return BadRequest(new Response<StudentUpdateDto>
                {
                    Succeeded = false,
                    Message = "Student ID mismatch.",
                    Errors = new[] { "Student ID mismatch." }
                });            }
            await _studentRepo.UpdateAsync(student);
        return Ok(new Response<StudentUpdateDto>(null, "Student updated successfully.", true));


        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Response<string>>> DeleteStudent(string id)
        {
            var existingStudent = await _studentRepo.GetByIdAsync(id);
            if (existingStudent == null)
            {
                return NotFound(new Response<string>("Student not found."));
            }
            await _studentRepo.DeleteAsync(id);
        return Ok(new Response<string>(null, "Student deleted successfully.", true));
        }

        // In BE/Controller/StudentController.cs
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