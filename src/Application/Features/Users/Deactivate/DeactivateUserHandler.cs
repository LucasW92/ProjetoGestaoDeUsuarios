using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Users.Deactivate;

public sealed class DeactivateUserHandler(UserManager<ApplicationUser> userManager)
{
    public async Task<Result> HandleAsync(
        DeactivateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.Id);

        if (user is null)
        {
            return Result.Failure(
                Error.NotFound("Users.NotFound", "Usuário não encontrado."));
        }

        if (!user.IsActive)
        {
            return Result.Success();
        }

        user.IsActive = false;
        user.DeactivatedAt = DateTimeOffset.UtcNow;

        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));

            return Result.Failure(
                Error.Validation("Users.DeactivationFailed", errors));
        }

        return Result.Success();
    }
}

