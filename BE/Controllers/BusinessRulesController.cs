using BE.Config;
using BE.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/config/businessrules")]
    public class BusinessRulesController : ControllerBase
    {
        private readonly IBusinessRulesService _businessRulesService;

        public BusinessRulesController(IBusinessRulesService businessRulesService)
        {
            _businessRulesService = businessRulesService;
        }

        // GET: api/config/businessrules
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_businessRulesService.GetSettings());
        }

        // PUT: api/config/businessrules
        [HttpPut]
        public IActionResult Update([FromBody] BusinessRulesSettings newSettings)
        {
            _businessRulesService.UpdateSettings(newSettings);
            return Ok(_businessRulesService.GetSettings());
        }
    }
}
