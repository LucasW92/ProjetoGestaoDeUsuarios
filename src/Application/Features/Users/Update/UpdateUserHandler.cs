using GestaoDeUsuarios.Application.Common;
using FluentValidation;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Users.Update;

public sealed class UpdateUserHandler(UserManager<ApplicationUser> userManager, IValidator<UpdateUserRequest> validator)
{
    public async Task<Result> HandleAsync(UpdateUserRequest request, CancellationToken cancellationToken = default)
    {

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));

            return Result.Failure(
                Error.Validation("Users.InvalidData", errors));
        }

        var user = await userManager.FindByIdAsync(request.Id);
        var (firstName, lastName) = NomeUsuarioMapper.Separar(request.Nome);

        if (user is null)
        {
            return Result.Failure(
                Error.NotFound("Users.NotFound", "Usuário não encontrado."));
        }

        var userWithEmail = await userManager.FindByEmailAsync(request.Email);

        if (userWithEmail is not null && userWithEmail.Id != user.Id)
        {
            return Result.Failure(
                Error.Conflict("Users.EmailTaken", "O email já está em uso por outro usuário."));
        }

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Email = request.Email;
        user.UserName = request.Email;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));

            return Result.Failure(
                Error.Validation("Users.UpdateFailed", errors));
        }

        return Result.Success();
    }
}
