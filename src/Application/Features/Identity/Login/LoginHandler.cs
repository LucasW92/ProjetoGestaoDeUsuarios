using GestaoDeUsuarios.Application.Abstractions.Identity;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Identity.Login;

public sealed class LoginHandler(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
{
    public async Task<Result<TokenResponse>> HandleAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure<TokenResponse>(Error.Validation("Auth.InvalidCredentials", "Email ou senha inválidos."));


        var isValidPassword = await userManager.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            return Result.Failure<TokenResponse>(Error.Validation("Auth.InvalidCredentials", "Email ou senha inválidos."));

        if (!user.IsActive)
        {
            return Result.Failure<TokenResponse>(
                Error.Validation("Auth.InvalidCredentials", "Email ou senha inválidos."));
        }

        var token = await tokenService.GenerateTokenAsync(user, cancellationToken);
        return Result.Success(token);
    }
}
