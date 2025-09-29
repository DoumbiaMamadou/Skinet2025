using System;
using System.Net;
using System.Text.Json;
using System.Xml.Serialization;

namespace API.Middleware;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                // Handle the exception and potentially write a custom response
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                // ... further error handling logic ...
            }
        }
    }
// // public class ExceptionMiddleware(IHostEnvironment env, RequestDelegate next)
// {
//     public async Task InvoAsync(HttpContext context)
//     {
//         try
//         {
//             await next(context);
//         }
//         catch (Exception ex)
//         {

//             await HandleExceptionAsync(context, ex, env);
//         }
//     }

//     private static Task HandleExceptionAsync(HttpContext context, Exception ex, IHostEnvironment env)
//     {
//         context.Response.ContentType = "application/json";
//         context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//         var response = env.IsDevelopment()
//             ? new Errors.ApiErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
//             : new Errors.ApiErrorResponse(context.Response.StatusCode, "Internal Server Error");

//         var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

//         var json = JsonSerializer.Serialize(response, options);

//         return context.Response.WriteAsync(json);
//     }
    
    
// }
