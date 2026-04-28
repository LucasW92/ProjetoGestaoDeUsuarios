using GestaoDeUsuarios.Application.Abstractions.Identity;
using GestaoDeUsuarios.Domain.Common;

namespace GestaoDeUsuarios.Application.Features.Identity.RefreshToken;

public sealed class RefreshTokenHandler(ITokenService tokenService)
{
    public async Task<Result<TokenResponse>> HandleAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var token = await tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken, cancellationToken);
            return Result.Success(token);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Failure<TokenResponse>(Error.Validation("Auth.InvalidRefreshToken", ex.Message));
        }
    }
}
