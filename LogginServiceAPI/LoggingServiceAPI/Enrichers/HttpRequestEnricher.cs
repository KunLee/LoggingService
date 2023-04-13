using Serilog.Core;
using Serilog.Events;

namespace LoggingServiceAPI.Enrichers
{
    public class HttpRequestEnricher : ILogEventEnricher
    {
        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string HttpRequestClientHostNamePropertyName = "HttpRequestClientIp";
        public const string HttpRequestClientUserNamePropertyName = "HttpRequestClientUserName";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpRequestEnricher(): this(new HttpContextAccessor())
        {
        }

        public HttpRequestEnricher(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));

            if (_httpContextAccessor.HttpContext?.Request == null)
                return;

            var userClientIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(userClientIp)) 
            {
                var httpRequestClientHostnameProperty = new LogEventProperty(HttpRequestClientHostNamePropertyName, new ScalarValue(userClientIp));
                logEvent.AddPropertyIfAbsent(httpRequestClientHostnameProperty);
            }

            if (!string.IsNullOrWhiteSpace(userName)) 
            {
                var httpRequestClientUsernameProperty = new LogEventProperty(HttpRequestClientUserNamePropertyName, new ScalarValue(userName));
                logEvent.AddPropertyIfAbsent(httpRequestClientUsernameProperty);
            }
        }
    }
}
