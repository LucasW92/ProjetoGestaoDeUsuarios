using System.Diagnostics;
using GestaoDeUsuarios.Api.Extensions;

namespace GestaoDeUsuarios.Api.Middleware;

public sealed class ResponseTimingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        context.SetResponseStopwatch(stopwatch);

        try
        {
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
        }
    }
}