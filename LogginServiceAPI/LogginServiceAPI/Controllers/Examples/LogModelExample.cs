using LogginServiceAPI.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Filters;

namespace LogginServiceAPI.Controllers.Examples
{
    public class LogModelExample : IExamplesProvider<LogRequest>
    {
        

        public LogRequest GetExamples()
        {
            var myData = new
            {
                Host = @"example.myhost.gr",
                UserName = "example",
                Password = "example",
                SourceDir = "example/export/zip/mypath/",
                FileName = "example.zip"
            };

            string jsonData = JsonConvert.SerializeObject(myData);

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
                        Message = "User Login Error",
                        UserId = "user1",
                        RequestId = "1",
                        AppName = "authentication_service",
                        ContextData = jsonData,
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
                        Message = "Web Service Error",
                        UserId = "000",
                        RequestId = "2",
                        AppName = "authentication_service",
                        ContextData = jsonData,
                        StackTrace = ""
                    }
                }
            };
        }
    }
}
