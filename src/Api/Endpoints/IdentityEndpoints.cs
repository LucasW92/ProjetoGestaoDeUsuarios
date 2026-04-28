using GestaoDeUsuarios.Api.Extensions;
using GestaoDeUsuarios.Application.Features.Identity.Login;
using GestaoDeUsuarios.Application.Features.Identity.RefreshToken;
using GestaoDeUsuarios.Application.Features.Identity.Register;

namespace GestaoDeUsuarios.Api.Endpoints;

public static class IdentityEndpoints
{
    public static void MapIdentityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity")
            .WithTags("Identidade")
            .AllowAnonymous();

        group.MapPost("/register", Register)
            .AddEndpointFilter<ValidationFilter<RegisterRequest>>()
            .WithName("Register")
            .WithSummary("Registrar um novo usuário")
            .WithDescription("Realiza o auto cadastro de um novo usuário sem exigir autenticação prévia.");

        group.MapPost("/login", Login)
            .AddEndpointFilter<ValidationFilter<LoginRequest>>()
            .WithName("Login")
            .WithSummary("Autenticar com email e senha");

        group.MapPost("/refresh", Refresh)
            .WithName("RefreshToken")
            .WithSummary("Renovar um token de acesso expirado");
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        RegisterHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, new { Mensagem = "Usuário cadastrado com sucesso." })
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        LoginHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, result.Value)
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> Refresh(
        RefreshTokenRequest request,
        RefreshTokenHandler handler,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, result.Value)
            : result.ToProblemDetails(httpContext);
    }
}
