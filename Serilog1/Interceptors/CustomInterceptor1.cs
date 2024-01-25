using Microsoft.AspNetCore.Mvc.Filters;
using Serilog.Context;
using System.Diagnostics;

namespace Serilog1.Interceptors
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class CustomInterceptor1 : Attribute, IAsyncResourceFilter
    {
        private ILogger<CustomInterceptor1> _logger;

        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var ms1 = new MemoryStream();
            context.HttpContext.Response.Body = ms1;

            await next();

            context.HttpContext.Response.OnCompleted(async () =>
            {
                var httpContext1 = context.HttpContext;
                if (httpContext1.Response.StatusCode > 299 || httpContext1.Response.StatusCode < 200)
                {
                    ms1.Seek(0, SeekOrigin.Begin);
                    var sr1 = new StreamReader(ms1);
                    var responseBody = sr1.ReadToEnd();
                    _logger = httpContext1.RequestServices.GetRequiredService<ILogger<CustomInterceptor1>>();
                    var traceId = httpContext1.TraceIdentifier ?? httpContext1.Request.Headers["X-TraceId"].FirstOrDefault();

                    using (LogContext.PushProperty(name: "ResponseBody1", value: responseBody, destructureObjects: true))
                    {
                        _logger.LogError("Bu requestte bir hata meydana geldi, interceptordan log gönderiyorum. Bu requestin traceId: {@TraceId}", traceId);
                    }
                    sr1.Dispose();
                    sr1.Close();
                }
                ms1.Dispose();
                ms1.Close();
            });

        }

        /*
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.responseBody = new MemoryStream();
            context.HttpContext.Response.Body = responseBody;
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            context.HttpContext.Response.OnCompleted(async () =>
            {
                responseBody.Seek(0, SeekOrigin.Begin);
                using (StreamReader sr = new StreamReader(responseBody))
                {
                    var responseBody = sr.ReadToEnd();
                    var httpContext1 = context.HttpContext;
                    if (httpContext1.Response.StatusCode > 299 || httpContext1.Response.StatusCode < 200)
                    {
                        _logger = httpContext1.RequestServices.GetRequiredService<ILogger<CustomInterceptor1>>();
                        var traceId = httpContext1.TraceIdentifier ?? httpContext1.Request.Headers["X-TraceId"].FirstOrDefault();

                        using (LogContext.PushProperty(name: "ResponseBody1", value: responseBody, destructureObjects: true))
                        {
                            _logger.LogError("Bu requestte bir hata meydana geldi, interceptordan log gönderiyorum. Bu requestin traceId: {TraceId}", traceId);
                        }
                    }
                }
                base.OnResultExecuted(context);
                responseBody.Dispose();
                responseBody.Close();
            });
        }
        */

    }
}
