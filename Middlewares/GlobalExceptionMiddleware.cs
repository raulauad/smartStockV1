using System.Net;
using System.Text.Json;

namespace SmartStockV1.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var (statusCode, message) = exception switch
            {
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Acceso no autorizado."),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "Ocurrio un error inesperado en el servidor.")
            };

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                error = message,
                traceId = context.TraceIdentifier,
                detail = _env.IsDevelopment() ? exception.Message : null

            };

            var json = JsonSerializer.Serialize(response, 
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            await context.Response.WriteAsync(json);
        }
    }
}
