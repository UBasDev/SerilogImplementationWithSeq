using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace Serilog1.Middlewares
{
    public class SerilogMiddleware1(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            using(LogContext.PushProperty("CorrelationId1", context.TraceIdentifier)) //Bu şekilde LogContext`ine bir property ve value ekleyebiliriz.
            {
                await _next(context);
            }
        }
    }
}
