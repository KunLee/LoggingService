using Moq;
using Serilog.Extensions.Logging;
using Serilog.Sinks.TestCorrelator;
using Serilog;
using LoggingService.Client.References;
using LoggingService.Client.LoggingService;
using LoggingService.Client.LoggingService.Models;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace LoggingService.Client.Test.ClientTests
{
    public class LoggingServiceClientTests
    {
        private readonly Mock<ILoggingService> _loggingService;
        public LoggingServiceClientTests()
        {
            _loggingService = new Mock<ILoggingService>();
            _loggingService.Setup(x => x.LoggingAsync(It.IsAny<LogRequest>())).Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task WhenClientTriggerLogging_Via_CallingLoggingService_ThenReturnCompletedAsync()
        {
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;

                var microsoftLogger = new SerilogLoggerFactory(logger)
                                            .CreateLogger<LoggingServiceClient>();

                var loggingApiClient = new LoggingServiceClient(_loggingService.Object, microsoftLogger);

                var result = await loggingApiClient.Send(new GetLogRequest
                {
                    Entries = new List<GetLogEntry>
                    {
                        new GetLogEntry{
                            LogLevel = "warning",
                            EnvironmentName= "dev",
                            HostName= "localhost",
                            InstanceId = "1c3be8ed-df30-47b4-8f1e-6e68ebd01f34",
                            LogSource = "user_service.object1.method1",
                            Message = "User Login Error",
                            UserId = "user1",
                            AppName = "authentication_service"
                        }
                    }
                });

                Assert.True(result);
            }
        }

        [Fact]
        public async Task WhenClientTriggerLogging_Via_CallingLoggingServiceWithException_ThenErrorLoggedAsync()
        {
            using (TestCorrelator.CreateContext())
            using (var logger = new LoggerConfiguration().WriteTo.Sink(new TestCorrelatorSink()).Enrich.FromLogContext().CreateLogger())
            {
                Log.Logger = logger;

                var microsoftLogger = new SerilogLoggerFactory(logger)
                                            .CreateLogger<LoggingServiceClient>();

                var loggingApiClient = new LoggingServiceClient(_loggingService.Object, microsoftLogger);

                var result = await loggingApiClient.Send(null);

                var logContext = TestCorrelator.GetLogEventsFromCurrentContext();

                var log = logContext.Should().ContainSingle();

                Assert.False(result);
            }
        }
    }
}
