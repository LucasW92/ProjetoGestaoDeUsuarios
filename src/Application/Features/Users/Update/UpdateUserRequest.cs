namespace GestaoDeUsuarios.Application.Features.Users.Update;

public sealed record UpdateUserRequest
(
    string Id,
    string Nome,
    string Email

);
