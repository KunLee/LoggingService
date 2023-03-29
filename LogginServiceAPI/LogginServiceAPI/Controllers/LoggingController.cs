using LogginServiceAPI.Controllers.Examples;
using LogginServiceAPI.Helpers;
using LogginServiceAPI.Models;
using LogginServiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace LogginServiceAPI.Controllers
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(LogRequest), typeof(LogModelExample))]
        public async Task<IActionResult> Post([FromBody] LogRequest request)
        {
            var result = await _loggingService.Log(request);

            return result ? Ok() : BadRequest("The request message is not valid.");
        }
    }
}
