using Serilog;
using Serilog.Sinks.TestCorrelator;
using FluentAssertions;
using Serilog.Context;
using LogginServiceAPI.Controllers;
using Serilog.Extensions.Logging;
using Microsoft.Extensions.Logging;
using LogginServiceAPI.Controllers.Examples;
using LogginServiceAPI.Models;

namespace LoggingServiceAPI.Test.ControllersTests
{
    public class LoggingControllerTests
    {
        [Fact]
        public async Task WhenClientTriggerLogging_Via_RequestWithPayload_ThenMessageLogged()
        {
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;

                var microsoftLogger = new SerilogLoggerFactory(logger)
                                            .CreateLogger<LoggingController>();

                var controller = new LoggingController(microsoftLogger);

                var request = new LogModelExample().GetExamples();

                await controller.Post(request);

                var context = TestCorrelator.GetLogEventsFromCurrentContext();
                context.Count().Should().Be(2);

                foreach (var item in context) 
                {
                    item.MessageTemplate.Text
                        .Should().Be("{MessageID}{@DataObject}");
                }
            }
        }

        [Fact]
        public async Task WhenClientTriggerLogging_Via_RequestWithEmpty_ThenLogError()
        {
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;

                var microsoftLogger = new SerilogLoggerFactory(logger)
                                            .CreateLogger<LoggingController>();

                var controller = new LoggingController(microsoftLogger);

                controller.Post(null);

                var context = TestCorrelator.GetLogEventsFromCurrentContext();

                context.Should().ContainSingle().Which.Level.Should().Be(Serilog.Events.LogEventLevel.Error);

                controller.Post(new LogRequest { Entries = new List<LogEntry>()});

                context.Count().Should().Be(2);
            }
        }

        public static void FooAsync()
        {
            using (LogContext.PushProperty("foo", "bar"))
            {
                Log.Information("foobar");
            }
        }

        [Fact]
        public void A_context_captures_LogEvents_even_in_sub_methods()
        {
            ///Arrange
            // I had issues with unit test seeing log events from other tests running at the same time so I recreate context in each test now
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;
                /*...*/
                /// Act
                FooAsync();
                /*...*/

                /// Assert 
                TestCorrelator.GetLogEventsFromCurrentContext().Should().ContainSingle().Which.MessageTemplate.Text.Should().Be("foobar");
            }
        }
    }
}
