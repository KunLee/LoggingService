using LogginServiceAPI.Controllers.Examples;
using LogginServiceAPI.Helpers;
using LogginServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace LogginServiceAPI.Controllers
{
    /// <summary>
    /// The logging controller is responsible for handling the remote API logging requests
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LoggingController : Controller
    {
        private readonly ILogger<LoggingController> _logger;
        public LoggingController(ILogger<LoggingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerRequestExample(typeof(LogRequest), typeof(LogModelExample))]
        public async Task<IActionResult> Post([FromBody] LogRequest request)
        {
            if (request == null || !request.Entries.Any())
            {
                var msg = "The request payload is empty or missing log entries.";
                _logger.LogError(msg);
                return BadRequest(msg);
            }

            await Task.Run(() => request.Entries
                      .ForEach(entry => _logger.Log(LogHelper.GetLogLevel(entry.LogLevel), 
                        "{MessageID}{@DataObject}", Guid.NewGuid(), entry)));

            return Ok();
        }
    }
}
