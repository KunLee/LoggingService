using LoggingService.Client.LoggingService;
using Microsoft.Extensions.DependencyInjection;

namespace LoggingService.Client.Extensions
{
    public static class LoggingServiceExtensions
    {
        public static IServiceCollection AddApiLoggingService(this IServiceCollection services, string url)
        {
            services.AddHttpClient("LoggingServiceHttpClient",
            c =>
            {
                c.BaseAddress = new Uri(uriString: url);
                c.Timeout = TimeSpan.FromMinutes(1); //  Can be set to Timeout.InfiniteTimeSpan to allow the TimeoutHandler to set timeout
            });

            services.AddSingleton<References.ILoggingService, References.LoggingService>(x => {

                var result = new References.LoggingService(x.GetRequiredService<IHttpClientFactory>().CreateClient("LoggingServiceHttpClient"));

                return result;
            }
            );

            services.AddSingleton<ILoggingServiceClient, LoggingServiceClient>();
            
            return services;
        }
    }
}
