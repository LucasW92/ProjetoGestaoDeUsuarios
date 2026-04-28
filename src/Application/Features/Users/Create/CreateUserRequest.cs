namespace GestaoDeUsuarios.Application.Features.Users.Create;

public sealed record CreateUserRequest(
    string Nome,
    string Email,
    string Password,
    string ConfirmPassword
);

