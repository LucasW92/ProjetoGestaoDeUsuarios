namespace GestaoDeUsuarios.Api.Extensions;

public static class ApiHttpResults
{
    public static IResult Ok(HttpContext httpContext, object? data)
    {
        return Results.Json(ApiResponseFactory.Create(httpContext, data));
    }

    public static IResult Created(HttpContext httpContext, string location, object? data)
    {
        httpContext.Response.Headers.Location = location;
        return Results.Json(ApiResponseFactory.Create(httpContext, data), statusCode: StatusCodes.Status201Created);
    }

    public static IResult BadRequest(HttpContext httpContext, object? data)
    {
        return Results.Json(ApiResponseFactory.Create(httpContext, data), statusCode: StatusCodes.Status400BadRequest);
    }

    public static IResult NotFound(HttpContext httpContext, object? data)
    {
        return Results.Json(ApiResponseFactory.Create(httpContext, data), statusCode: StatusCodes.Status404NotFound);
    }

    public static IResult Conflict(HttpContext httpContext, object? data)
    {
        return Results.Json(ApiResponseFactory.Create(httpContext, data), statusCode: StatusCodes.Status409Conflict);
    }

    public static IResult ServerError(HttpContext httpContext, object? data)
    {
        return Results.Json(ApiResponseFactory.Create(httpContext, data), statusCode: StatusCodes.Status500InternalServerError);
    }
}