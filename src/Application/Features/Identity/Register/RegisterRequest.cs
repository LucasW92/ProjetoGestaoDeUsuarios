namespace GestaoDeUsuarios.Application.Features.Identity.Register;

public sealed record RegisterRequest(
    string Nome,
    string Email,
    string Password,
    string ConfirmPassword);
