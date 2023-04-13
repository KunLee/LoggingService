using LoggingServiceAPI.Enrichers;
using LoggingServiceAPI.Extensions;
using LoggingServiceAPI.Middlewares;
using LoggingServiceAPI.Models;
using LoggingServiceAPI.Models.Utilities;
using LoggingServiceAPI.Services;
using Microsoft.Extensions.DependencyModel;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateBootstrapLogger();


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext()
    .Enrich.With<EventTypeEnricher>()
    .Enrich.With<HttpRequestEnricher>()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console());

// Add Serilog Initialization
//Log.Logger = new LoggerConfiguration()
//        .ReadFrom.Configuration(builder.Configuration)
//        .WriteTo.CustomSink()
//        .Enrich.With<EventTypeEnricher>()
//        .Enrich.FromLogContext()
//        .Enrich.WithMachineName()
//        .Enrich.WithEnvironmentName()
//        .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamples();


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Logging API", Version = "v1" });
    c.ExampleFilters();
});

builder.Services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMessageUtilities<LogRequest>, LogMessageUtilities>();
builder.Services.AddSingleton<ILoggingService, LoggingService>();

try
{
    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware(typeof(HttpContextInfoMiddleware));

    app.UseMiddleware(typeof(ExceptionHandlingMiddleware));

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}