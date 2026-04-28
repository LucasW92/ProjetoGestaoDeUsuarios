using GestaoDeUsuarios.Application.Common;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Users.Create;

public sealed class CreateUserHandler(UserManager<ApplicationUser> userManager)
{
    public async Task<Result<CreateUserResponse>> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        var (firstName, lastName) = NomeUsuarioMapper.Separar(request.Nome);

        if (existingUser is not null)
        {
            return Result.Failure<CreateUserResponse>(
                Error.Conflict("Users.EmailTaken", "Já existe um usuário com este email."));
        }

        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = request.Email,
            UserName = request.Email,
            IsActive = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure<CreateUserResponse>(
                Error.Validation("Users.CreationFailed", errors));
        }

        return Result.Success(new CreateUserResponse(
            user.Id,
            NomeUsuarioMapper.Juntar(user.FirstName, user.LastName),
            user.Email!,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt));
    }
}

