using Serilog;
using System.Diagnostics;

namespace UserPermissionsAdmin.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            Log.Information($"Incoming Request: {context.Request.Method} {context.Request.Path} at {DateTime.UtcNow}");

            await _next(context);

            stopwatch.Stop();

            Log.Information($"Response: {context.Request.Method} {context.Request.Path} " +
                $"with Status Code {context.Response.StatusCode} processed in {stopwatch.ElapsedMilliseconds }ms at {DateTime.UtcNow}");
        }
    }
}
