using BE.Exceptions.Faculty;
using BE.Interface;
using BE.Models;
using BE.Repository;
using BE.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyRepository _repository;
        private readonly IStudentRepository _studentRepository;

        public FacultyController(IFacultyRepository repository, IStudentRepository studentRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<Faculty>>>> GetAll()
        {
            var faculties = await _repository.GetAllAsync();
            return Ok(new Response<IEnumerable<Faculty>>(faculties));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<Faculty>>> GetById(int id)
        {
            var faculty = await _repository.GetByIdAsync(id);
            if (faculty == null)
                return NotFound(new Response<Faculty>(null, "Faculty not found", false));
            return Ok(new Response<Faculty>(faculty));
        }

        [HttpPost]
        public async Task<ActionResult<Response<Faculty>>> Create(Faculty faculty)
        {
            var created = await _repository.CreateAsync(faculty);
            return CreatedAtAction(nameof(GetById), new { id = created.FacultyId }, new Response<Faculty>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Faculty faculty)
        {
            if (id != faculty.FacultyId)
                return BadRequest(new Response<Faculty>(null, "Invalid ID", false));

            await _repository.UpdateAsync(faculty);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var students = await _studentRepository.GetStudentsByFacultyIdAsync(id);
            if (students.Any())
                throw new FacultyCantDelete(id);
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
} 