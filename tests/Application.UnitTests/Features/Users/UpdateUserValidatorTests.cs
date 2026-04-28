using GestaoDeUsuarios.Application.Features.Users.Update;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class UpdateUserValidatorTests
{
    [Fact]
    public void Validate_Should_Pass_When_Request_Is_Valid()
    {
        var validator = new UpdateUserValidator();
        var request = new UpdateUserRequest(Guid.NewGuid().ToString(), "Jane Doe", "jane@cwm.dev");

        var result = validator.Validate(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Should_Fail_When_Nome_Is_Empty()
    {
        var validator = new UpdateUserValidator();
        var request = new UpdateUserRequest(Guid.NewGuid().ToString(), string.Empty, "jane@cwm.dev");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateUserRequest.Nome));
    }

    [Fact]
    public void Validate_Should_Fail_When_Email_Is_Invalid()
    {
        var validator = new UpdateUserValidator();
        var request = new UpdateUserRequest(Guid.NewGuid().ToString(), "Jane Doe", "invalid-email");

        var result = validator.Validate(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(UpdateUserRequest.Email));
    }
}