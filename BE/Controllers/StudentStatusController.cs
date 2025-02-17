using BE.Interface;
using BE.Models;
using BE.Repository;
using BE.Utils;
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
        public async Task<ActionResult<Response<IEnumerable<StudentStatus>>>> GetAll()
        {
            var statuses = await _repository.GetAllAsync();
            return Ok(new Response<IEnumerable<StudentStatus>>(statuses));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<StudentStatus>>> GetById(int id)
        {
            var status = await _repository.GetByIdAsync(id);
            if (status == null)
                return NotFound(new Response<StudentStatus>(null, "Status not found", false));
            return Ok(new Response<StudentStatus>(status));
        }

        [HttpPost]
        public async Task<ActionResult<Response<StudentStatus>>> Create(StudentStatus status)
        {
            var created = await _repository.CreateAsync(status);
            return CreatedAtAction(nameof(GetById), new { id = created.StatusId }, new Response<StudentStatus>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, StudentStatus status)
        {
            if (id != status.StatusId)
                return BadRequest(new Response<StudentStatus>(null, "Invalid ID", false));

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