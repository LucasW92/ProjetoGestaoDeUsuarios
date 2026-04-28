using System.Diagnostics;

namespace GestaoDeUsuarios.Api.Extensions;

public static class HttpContextResponseTimingExtensions
{
    private const string StopwatchKey = "response_stopwatch";

    public static void SetResponseStopwatch(this HttpContext httpContext, Stopwatch stopwatch)
    {
        httpContext.Items[StopwatchKey] = stopwatch;
    }

    public static long GetElapsedMilliseconds(this HttpContext httpContext)
    {
        return httpContext.Items.TryGetValue(StopwatchKey, out var value) && value is Stopwatch stopwatch
            ? stopwatch.ElapsedMilliseconds
            : 0L;
    }
}