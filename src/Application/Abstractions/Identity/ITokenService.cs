namespace GestaoDeUsuarios.Application.Abstractions.Identity;

using GestaoDeUsuarios.Domain.Entities;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default);
    Task<TokenResponse> RefreshTokenAsync(string accessToken, string refreshToken, CancellationToken cancellationToken = default);
}

public sealed record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
