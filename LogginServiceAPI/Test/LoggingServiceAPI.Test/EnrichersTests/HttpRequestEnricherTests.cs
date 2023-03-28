using Moq;
using Serilog.Core;
using LogginServiceAPI.Enrichers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog.Parsing;
using Microsoft.AspNetCore.Http;

namespace LoggingServiceAPI.Test.EnrichersTests
{
    public class HttpRequestEnricherTests
    {
        private HttpRequestEnricher _enricher;
        private Mock<ILogEventPropertyFactory> _propertyFactory;
        private HttpContextAccessor _httpContextAccessor;

        public HttpRequestEnricherTests()
        {
            _propertyFactory = new Mock<ILogEventPropertyFactory>();

            _propertyFactory.Setup(x => x.CreateProperty(It.IsAny<string>(), It.IsAny<object>(), false))
                .Returns<string, object, bool>((a,b,c) => new LogEventProperty(a, new ScalarValue(b)));

            _httpContextAccessor = new HttpContextAccessor();
            _httpContextAccessor.HttpContext = new DefaultHttpContext();
            _enricher = new HttpRequestEnricher(_httpContextAccessor);
        }

        [Fact]
        public void WhenEnrichWithNewEvent_ThenEventTypeAdded() 
        {
            var logEvent = new LogEvent(DateTimeOffset.UtcNow, LogEventLevel.Verbose, null,
                    new MessageTemplate("atemplate", new List<MessageTemplateToken>()),
                    new List<LogEventProperty>
                    {
                        new LogEventProperty("aname1", new ScalarValue("avalue")),
                        new LogEventProperty("aname2", new ScalarValue(42))
                    });
            _enricher.Enrich(logEvent, _propertyFactory.Object);

            Assert.Equal(2, logEvent.Properties.Count);

            _httpContextAccessor.HttpContext.Request.Host = new HostString("localhost");

            _enricher.Enrich(logEvent, _propertyFactory.Object);

            Assert.Equal(3, logEvent.Properties.Count);
        }
    }
}
