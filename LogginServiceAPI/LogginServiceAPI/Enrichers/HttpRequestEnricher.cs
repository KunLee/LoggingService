using Serilog.Core;
using Serilog.Events;

namespace LogginServiceAPI.Enrichers
{
    public class HttpRequestEnricher : ILogEventEnricher
    {
        /// <summary>
        /// The property name added to enriched log events.
        /// </summary>
        public const string HttpRequestClientHostNamePropertyName = "HttpRequestClientHostName";
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
            if (logEvent == null) throw new ArgumentNullException("logEvent");

            if (_httpContextAccessor.HttpContext?.Request == null)
                return;

            var userHostName = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var userName = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrWhiteSpace(userHostName)) 
            {
                var httpRequestClientHostnameProperty = new LogEventProperty(HttpRequestClientHostNamePropertyName, new ScalarValue(userHostName));
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
