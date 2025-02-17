using BE.Interface;
using BE.Models;
using BE.Repository;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationProgramController : ControllerBase
    {
        private readonly IApplicationProgramRepository _repository;

        public ApplicationProgramController(IApplicationProgramRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationProgram>>> GetAll()
        {
            var programs = await _repository.GetAllAsync();
            return Ok(programs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationProgram>> GetById(int id)
        {
            var program = await _repository.GetByIdAsync(id);
            if (program == null)
                return NotFound();
            return Ok(program);
        }

        [HttpPost]
        public async Task<ActionResult<ApplicationProgram>> Create(ApplicationProgram program)
        {
            var created = await _repository.CreateAsync(program);
            return CreatedAtAction(nameof(GetById), new { id = created.ProgramId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApplicationProgram program)
        {
            if (id != program.ProgramId)
                return BadRequest();

            await _repository.UpdateAsync(program);
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