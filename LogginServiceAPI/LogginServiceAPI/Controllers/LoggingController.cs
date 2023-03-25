using LogginServiceAPI.Controllers.Examples;
using LogginServiceAPI.Helpers;
using LogginServiceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Parsing;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;

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
        //[ProducesResponseType(200)]
        //[ProducesResponseType(400)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerRequestExample(typeof(LogRequest), typeof(LogModelExample))]
        public async Task<IActionResult> Post([FromBody] LogRequest request)
        {
            if (request == null || !request.Entries.Any())
            {
                return BadRequest("The request payload is empty or missing log entries.");
            }

            //await Task.Run(() => request.Entries
            //          .ForEach(entry => _logger.Log(LogHelper.GetLogLevel(entry.LogLevel), entry.Message, 
            //                typeof(LogEntry).GetProperties().Select(x => new LogEventProperty(x.Name, new ScalarValue(x.GetValue(entry)))))));

            await Task.Run(() => request.Entries
                      .ForEach(entry => _logger.Log(LogHelper.GetLogLevel(entry.LogLevel), "Client Data:{@DataObject}", entry)));

            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LogModelExample))]
        public async Task<IReadOnlyList<LogEntry>> Get()
        {
            return await Task.FromResult(new List<LogEntry>());
        }
    }
}
