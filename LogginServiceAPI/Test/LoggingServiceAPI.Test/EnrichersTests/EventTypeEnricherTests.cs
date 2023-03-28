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

namespace LoggingServiceAPI.Test.EnrichersTests
{
    public class EventTypeEnricherTests
    {
        private EventTypeEnricher _enricher;
        private Mock<ILogEventPropertyFactory> _propertyFactory;

        public EventTypeEnricherTests()
        {
            _propertyFactory = new Mock<ILogEventPropertyFactory>();

            _propertyFactory.Setup(x => x.CreateProperty(It.IsAny<string>(), It.IsAny<object>(), false))
                .Returns<string, object, bool>((a,b,c) => new LogEventProperty(a, new ScalarValue(b)));
            _enricher = new EventTypeEnricher();
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

            Assert.Equal(4, logEvent.Properties.Count);
        }
    }
}
