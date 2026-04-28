using GestaoDeUsuarios.Application.Common;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GestaoDeUsuarios.Application.Features.Identity.Register;

public sealed class RegisterHandler(UserManager<ApplicationUser> userManager)
{
    public async Task<Result> HandleAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);
        var (firstName, lastName) = NomeUsuarioMapper.Separar(request.Nome);
        if (existingUser is not null)
            return Result.Failure(Error.Conflict("Auth.EmailTaken", "Já existe um usuário com este email."));

        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            Email = request.Email,
            UserName = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result.Failure(Error.Validation("Auth.RegistrationFailed", errors));
        }

        return Result.Success();
    }
}
