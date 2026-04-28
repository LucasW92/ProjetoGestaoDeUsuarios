namespace GestaoDeUsuarios.Application.Features.Identity.Login;

using GestaoDeUsuarios.Application.Abstractions.Identity;

public sealed record LoginRequest(string Email, string Password);
