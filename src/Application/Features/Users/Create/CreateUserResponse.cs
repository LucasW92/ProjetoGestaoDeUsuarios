namespace GestaoDeUsuarios.Application.Features.Users.Create;

public sealed record CreateUserResponse(
    string Id,
    string Nome,
    string Email,
    bool Ativo,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
    );