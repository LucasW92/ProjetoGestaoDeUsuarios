using FluentValidation;

namespace GestaoDeUsuarios.Application.Features.Users.Create;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
{

    public CreateUserValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("As senhas não coincidem.");

    }

}
