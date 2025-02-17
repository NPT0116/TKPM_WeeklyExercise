using BE.Interface;
using BE.Models;
using BE.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyRepository _repository;

        public FacultyController(IFacultyRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Faculty>>> GetAll()
        {
            var faculties = await _repository.GetAllAsync();
            return Ok(faculties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Faculty>> GetById(int id)
        {
            var faculty = await _repository.GetByIdAsync(id);
            if (faculty == null)
                return NotFound();
            return Ok(faculty);
        }

        [HttpPost]
        public async Task<ActionResult<Faculty>> Create(Faculty faculty)
        {
            var created = await _repository.CreateAsync(faculty);
            return CreatedAtAction(nameof(GetById), new { id = created.FacultyId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Faculty faculty)
        {
            if (id != faculty.FacultyId)
                return BadRequest();

            await _repository.UpdateAsync(faculty);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
} 