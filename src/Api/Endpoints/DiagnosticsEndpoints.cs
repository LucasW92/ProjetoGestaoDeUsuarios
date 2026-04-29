namespace GestaoDeUsuarios.Api.Endpoints;

public static class DiagnosticsEndpoints
{
    public static void MapDiagnosticsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/diagnostics")
            .WithTags("Diagnóstico")
            .AllowAnonymous();

        group.MapGet("/throw", Throw)
            .WithName("ThrowUnhandledException")
            .WithSummary("Lançar exceção não tratada")
            .WithDescription("Endpoint de desenvolvimento para validar o tratamento global de exceções.");
    }

    private static IResult Throw()
    {
        throw new InvalidOperationException("Exceção de teste disparada manualmente.");
    }
}