using GestaoDeUsuarios.Application.Features.Users;
using GestaoDeUsuarios.Application.Common;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Users.GetById;

public sealed class GetUserByIdHandler(UserManager<ApplicationUser> userManager)
{
    public async Task<Result<UserResponse>> HandleAsync(
        GetUserByIdRequest request,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(request.Id);

        if (user is null)
        {
            return Result.Failure<UserResponse>(
                Error.NotFound("Users.NotFound", "Usuário não encontrado."));
        }

        return Result.Success(new UserResponse(
            user.Id,
            NomeUsuarioMapper.Juntar(user.FirstName, user.LastName),
            user.Email!,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt));
    }
}

