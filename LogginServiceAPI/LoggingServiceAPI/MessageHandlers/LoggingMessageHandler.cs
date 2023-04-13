using System.Diagnostics;

namespace LoggingServiceAPI.MessageHandlers
{
    public class LoggingMessageHandler : DelegatingHandler
    {
        public LoggingMessageHandler()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            await TryEncryptRequestContent(request);

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }

        private async Task TryEncryptRequestContent(HttpRequestMessage request)
        {
            if (request.Content == null)
            {
                return;
            }

            var requestContent = await GetRequestContent(request);

            if (string.IsNullOrEmpty(requestContent))
            {
                return;
            }
        }

        /// <summary>
        /// Extracts the contents of the body of the request
        /// </summary>
        private async Task<string> GetRequestContent(HttpRequestMessage request)
        {
            await request.Content.LoadIntoBufferAsync();

            var content = await request.Content.ReadAsStringAsync();

            return content;
        }
    }
}
