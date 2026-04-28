using GestaoDeUsuarios.Api.Contracts;

namespace GestaoDeUsuarios.Api.Extensions;

public static class ApiResponseFactory
{
    public static ApiResponseEnvelope<T> Create<T>(HttpContext httpContext, T? data)
    {
        return new ApiResponseEnvelope<T>(
            data,
            DateTime.Now,
            $"{httpContext.GetElapsedMilliseconds()} ms");
    }
}