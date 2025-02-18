using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers;


 [Route("api/[controller]")]
    [ApiController]
    public class AppInfoController : ControllerBase
    {
        [HttpGet("version")]
        public IActionResult GetVersion()
        {
            // Get the current assembly.
            var assembly = Assembly.GetExecutingAssembly();

            // Get the version from the assembly name.
            var version = assembly.GetName().Version?.ToString() ?? "Unknown";

            // Get the build date by checking the last write time of the assembly.
            // This is a common trick to estimate the build date.
            var buildDate = System.IO.File.GetLastWriteTime(assembly.Location).ToString("yyyy-MM-dd HH:mm:ss");

            return Ok(new 
            { 
                Version = version, 
                BuildDate = buildDate 
            });
        }
    }