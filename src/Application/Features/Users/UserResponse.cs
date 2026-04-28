namespace GestaoDeUsuarios.Application.Features.Users;

public sealed record UserResponse(
    string Id,
    string Nome,
    string Email,
    bool Ativo,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
    );

