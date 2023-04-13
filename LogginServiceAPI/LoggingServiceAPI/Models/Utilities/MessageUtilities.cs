using LoggingServiceAPI.Helpers;
using NETCore.Encrypt;

namespace LoggingServiceAPI.Models.Utilities
{
    /// <summary>
    /// The utilities for validation and properties encryption on Log Request 
    /// </summary>
    public class LogMessageUtilities : IMessageUtilities<LogRequest>
    {
        private readonly IConfiguration _config;

        public LogMessageUtilities(IConfiguration config) 
        {
            _config = config;
        }   
        public LogRequest Encrypt(LogRequest request)
        {
            var key = _config["AES:Key"];

            foreach (var item in request.Entries) 
            {
                var iv = MessageHelper.RandomString(16);

                if (!String.IsNullOrEmpty(item.UserId)) 
                {
                    item.UserId = EncryptProvider.AESEncrypt($"{item.UserId}{iv}", key, iv);
                }

                if (!String.IsNullOrEmpty(item.ContextData))
                {
                    item.ContextData = EncryptProvider.AESEncrypt($"{item.ContextData}{iv}", key, iv);
                }
            }

            return request;
        }

        public bool Validate(LogRequest request)
        {
            if (request.Entries == null || request.Entries.Count == 0) return false;
            
            foreach (var item in request.Entries) 
            {
                if (String.IsNullOrEmpty(item.LogLevel)) return false;
                if (String.IsNullOrEmpty(item.Message)) return false;
            }
            return true;
        }
    }
}
