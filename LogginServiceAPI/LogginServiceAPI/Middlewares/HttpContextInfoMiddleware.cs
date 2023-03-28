using Serilog.Context;
using System.Text;

namespace LogginServiceAPI.Middlewares
{
    /// <summary>
    /// Add the current http request to the Serilog context.
    /// </summary>
    public class HttpContextInfoMiddleware
    {
        private const string HttpRequestPropertyName = "HttpRequest";
        readonly RequestDelegate _next;

        public HttpContextInfoMiddleware(RequestDelegate next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var httpRequestInfo = await GetHttpRequestInfoAsync(httpContext);

            // Push the user name into the log context so that it is included in all log entries
            using (LogContext.PushProperty(HttpRequestPropertyName, httpRequestInfo, true))
            {
                await _next(httpContext);
            }
        }

        private async Task<HttpContextInfo> GetHttpRequestInfoAsync(HttpContext httpContext)
        {
            var httpRequest = httpContext?.Request;

            if (httpRequest == null)
            {
                return null;
            }

            string body = "";

            if (httpRequest.ContentLength.HasValue && httpRequest.ContentLength > 0)
            {
                httpRequest.EnableBuffering();

                using (var reader = new StreamReader(httpRequest.Body, Encoding.UTF8, false, 1024, true))
                {
                    body = await reader.ReadToEndAsync();
                }

                // Reset the request body stream position so the next middleware can read it
                httpRequest.Body.Position = 0;
            }

            return new HttpContextInfo()
            {
                Host = httpRequest.Host.ToString(),
                Path = httpRequest.Path,
                Scheme = httpRequest.Scheme,
                Method = httpRequest.Method,
                Protocol = httpRequest.Protocol,
                QueryString = httpRequest.Query.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Headers = httpRequest.Headers
                            .Where(x => x.Key != "Cookie") // remove Cookie from header since it is analysed separatly
                            .ToDictionary(x => x.Key, y => y.Value.ToString()),
                Cookies = httpRequest.Cookies.ToDictionary(x => x.Key, y => y.Value.ToString()),
                Body = body
            };
        }
    }
    internal class HttpContextInfo
    {
        public string Host { get; set; }
        public string Path { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public string Protocol { get; set; }
        public Dictionary<string, string> QueryString { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public Dictionary<string, string> Cookies { get; set; }
        public string Body { get; set; }
    }
}
