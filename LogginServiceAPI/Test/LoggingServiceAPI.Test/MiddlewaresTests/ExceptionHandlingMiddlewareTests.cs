using Serilog.Sinks.TestCorrelator;
using Serilog;
using FluentAssertions;
using LogginServiceAPI.Middlewares;
using Serilog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace LoggingServiceAPI.Test.MiddlewaresTests
{
    public class ExceptionHandlingMiddlewareTests
    {
        [Fact]
        public async Task WhenExceptionThrown_ThenHandlingAndLogging()
        {
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;
                var microsoftLogger = new SerilogLoggerFactory(logger)
                                            .CreateLogger<ExceptionHandlingMiddleware>();

                var requestDelegate = new RequestDelegate((httpContext) => throw new Exception("An exception thrown"));

                var exceptionHandlingMiddleware = new ExceptionHandlingMiddleware(requestDelegate, microsoftLogger);

                var httpContext = new DefaultHttpContext();

                httpContext.Response.Body = new MemoryStream();

                await exceptionHandlingMiddleware.Invoke(httpContext);

                var logContext = TestCorrelator.GetLogEventsFromCurrentContext();

                logContext.Should().ContainSingle()
                    .Which.MessageTemplate.Text.Contains("An exception thrown");
            }
        }
    }
}
