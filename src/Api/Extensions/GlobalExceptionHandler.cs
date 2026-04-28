using Microsoft.AspNetCore.Diagnostics;

namespace GestaoDeUsuarios.Api.Extensions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Ocorreu uma exceção não tratada: {Message}", exception.Message);

        var errorResponse = new
        {
            CodigoErro = "Server.UnexpectedError",
            Mensagem = httpContext.RequestServices
                .GetRequiredService<IHostEnvironment>()
                .IsDevelopment()
                    ? exception.Message
                    : "Tente novamente mais tarde ou entre em contato com o suporte.",
            TipoErro = "Failure"
        };

        var result = ApiHttpResults.ServerError(httpContext, errorResponse);
        await result.ExecuteAsync(httpContext);
        return true;
    }
}
