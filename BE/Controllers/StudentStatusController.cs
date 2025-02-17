using BE.Interface;
using BE.Models;
using BE.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentStatusController : ControllerBase
    {
        private readonly IStudentStatusRepository _repository;

        public StudentStatusController(IStudentStatusRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentStatus>>> GetAll()
        {
            var statuses = await _repository.GetAllAsync();
            return Ok(statuses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentStatus>> GetById(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null)
                return NotFound();
            return Ok(status);
        }

        [HttpPost]
        public async Task<ActionResult<StudentStatus>> Create(StudentStatus status)
        {
            var created = await _repository.CreateAsync(status);
            return CreatedAtAction(nameof(GetById), new { id = created.StatusId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentStatus status)
        {
            if (id != status.StatusId)
                return BadRequest();

            await _repository.UpdateAsync(status);
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