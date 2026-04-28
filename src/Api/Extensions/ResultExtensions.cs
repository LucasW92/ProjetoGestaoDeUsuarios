using GestaoDeUsuarios.Domain.Common;

namespace GestaoDeUsuarios.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result, HttpContext httpContext)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Não é possível converter um resultado de sucesso em problema.");

        var errorResponse = new
        {
            CodigoErro = result.Error!.Code,
            Mensagem = result.Error.Message,
            TipoErro = result.Error.Type.ToString()
        };

        return result.Error.Type switch
        {
            ErrorType.NotFound => ApiHttpResults.NotFound(httpContext, errorResponse),
            ErrorType.Validation => ApiHttpResults.BadRequest(httpContext, errorResponse),
            ErrorType.Conflict => ApiHttpResults.Conflict(httpContext, errorResponse),
            _ => ApiHttpResults.ServerError(httpContext, errorResponse)
        };
    }
}
