namespace GestaoDeUsuarios.Application.Features.Identity.RefreshToken;

using GestaoDeUsuarios.Application.Abstractions.Identity;

public sealed record RefreshTokenRequest(string AccessToken, string RefreshToken);
