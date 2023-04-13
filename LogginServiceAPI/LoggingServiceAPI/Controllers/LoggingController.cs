using LoggingServiceAPI.Controllers.Examples;
using LoggingServiceAPI.Exceptions;
using LoggingServiceAPI.Helpers;
using LoggingServiceAPI.Models;
using LoggingServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Mime;
using System.Reflection;
using System.Resources;

namespace LoggingServiceAPI.Controllers
{
    /// <summary>
    /// The logging controller is responsible for handling the remote API logging requests
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoggingController : Controller
    {
        private readonly ILoggingService _loggingService;
        public LoggingController(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(LogRequest), typeof(LogModelExample))]
        public async Task<IActionResult> Post([FromBody] LogRequest request)
        {
            var result = await _loggingService.Log(request);

            return result ? Ok() : 
                BadRequest(new ResourceManager($"LoggingServiceAPI.Resources.Strings", 
                    typeof(LoggingController).Assembly).GetString(nameof(BadRequest)));
        }
    }
}
