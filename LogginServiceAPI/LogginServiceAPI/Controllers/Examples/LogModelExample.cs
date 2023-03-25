using LogginServiceAPI.Models;
using Swashbuckle.AspNetCore.Filters;

namespace LogginServiceAPI.Controllers.Examples
{
    public class LogModelExample : IExamplesProvider<LogRequest>
    {
        public LogRequest GetExamples()
        {
            return new LogRequest
            {
                Entries = new List<LogEntry>
                {
                    new LogEntry {
                        TimeStamp = DateTime.Now.ToLocalTime(),
                        LogLevel = "warning",
                        EnvironmentName= "dev",
                        HostName= "localhost",
                        InstanceId = "1c3be8ed-df30-47b4-8f1e-6e68ebd01f34",
                        LogFileName = "user_service.log",
                        LogSource = "user_service.object1.method1",
                        Message = "User {UserId} Login Error",
                        UserId = "user1",
                        RequestId = "1",
                        AppName = "authentication_service",
                        ContextData = "{\"ip_address\": \"192.168.0.1\", \"url\": \"/api/users\", \"http_method\": \"GET\"}\r\n",
                        StackTrace = "Traceback (most recent call last):\n File 'user_service.py', line 42, in <module>\n raise ValueError('Invalid email address')\nValueError: Invalid email address"
                    },
                    new LogEntry {
                        TimeStamp = DateTime.Now.ToLocalTime(),
                        LogLevel = "error",
                        EnvironmentName= "prod",
                        HostName= "WebServer1",
                        InstanceId = "1c3be8ed-df30-47b4-8f1e-6e68ebd01f34",
                        LogFileName = "user_service.log",
                        LogSource = "web_service.object1.method1",
                        Message = "Web Service {AppName} Error",
                        UserId = "000",
                        RequestId = "2",
                        AppName = "authentication_service",
                        ContextData = "{\"ip_address\": \"192.168.0.1\", \"url\": \"/api/data\", \"http_method\": \"Post\"}\r\n",
                        StackTrace = ""
                    }
                }
            };
        }
    }
}
