using BE.Exceptions.ApplicationProgram;
using BE.Interface;
using BE.Models;
using BE.Repository;
using BE.Utils;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationProgramController : ControllerBase
    {
        private readonly IApplicationProgramRepository _repository;
        private readonly IStudentRepository _studentRepository;

        public ApplicationProgramController(IApplicationProgramRepository repository, IStudentRepository studentRepository)
        {
            _repository = repository;
            _studentRepository = studentRepository;
        }
        [HttpGet]
        public async Task<ActionResult<Response<IEnumerable<ApplicationProgram>>>> GetAll()
        {
            var programs = await _repository.GetAllAsync();
            return Ok(new Response<IEnumerable<ApplicationProgram>>(programs));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Response<ApplicationProgram>>> GetById(int id)
        {
            var program = await _repository.GetByIdAsync(id);
            if (program == null)
                return NotFound(new Response<ApplicationProgram>(null));
            return Ok(new Response<ApplicationProgram>(program));
        }

        [HttpPost]
        public async Task<ActionResult<Response<ApplicationProgram>>> Create(ApplicationProgram program)
        {
            var created = await _repository.CreateAsync(program);
            return CreatedAtAction(nameof(GetById), new { id = created.ProgramId }, new Response<ApplicationProgram>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ApplicationProgram program)
        {
            if (id != program.ProgramId)
                return BadRequest(new Response<ApplicationProgram>(null, "Invalid ID", false));

            await _repository.UpdateAsync(program);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var students = await _studentRepository.GetStudentsByProgramIdAsync(id);
            if (students.Any())
                throw new ProgramCantDelete(id);
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
} 