﻿using Newtonsoft.Json;
using System.Net;

namespace LoggingServiceAPI.Middlewares
{
    /// <summary>
    /// The exception handlling middleware which is responsible for catching and handling exceptions that occur in the Web API pipeline
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        public RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware
            (RequestDelegate requestDelegate, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleException(context, ex);
            }
        }
        private Task HandleException(HttpContext context, Exception ex)
        {
            _logger.LogError(ex.ToString());
            var errorMessageObject =
                new { Message = ex.Message, Code = "system_error" };

            var errorMessage = JsonConvert.SerializeObject(errorMessageObject);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(errorMessage);
        }
    }
}
