using GestaoDeUsuarios.Application.Features.Users.Create;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class CreateUserValidatorTests
{
    [Fact]
    public void Validate_Should_Pass_When_Request_Is_Valid()
    {
        var validator = new CreateUserValidator();
        var request = new CreateUserRequest("Jane Doe", "jane@cwm.dev", "Valid@123", "Valid@123");

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Should_Fail_When_Passwords_Do_Not_Match()
    {
        var validator = new CreateUserValidator();
        var request = new CreateUserRequest("Jane Doe", "jane@cwm.dev", "Valid@123", "Other@123");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage == "As senhas não coincidem.");
    }

    [Fact]
    public void Validate_Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new CreateUserValidator();
        var request = new CreateUserRequest("Jane Doe", "invalid-email", "Valid@123", "Valid@123");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateUserRequest.Email));
    }

    [Fact]
    public void Validate_Should_Fail_When_Nome_Is_Empty()
    {
        var validator = new CreateUserValidator();
        var request = new CreateUserRequest(string.Empty, "jane@cwm.dev", "Valid@123", "Valid@123");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(CreateUserRequest.Nome));
    }
}