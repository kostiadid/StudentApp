using System.Text.Json;
using FluentValidation;

namespace StudentApp.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, "Validation failed", ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (KeyNotFoundException ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, int statusCode, string message, IEnumerable<string>? details = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var payload = new
            {
                statusCode,
                message,
                details
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
