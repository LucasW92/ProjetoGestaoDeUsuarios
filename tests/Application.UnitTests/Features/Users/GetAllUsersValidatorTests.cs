using GestaoDeUsuarios.Application.Features.Users.GetAll;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class GetAllUsersValidatorTests
{
    [Fact]
    public void Validate_Should_Pass_When_Request_Is_Valid()
    {
        var validator = new GetAllUsersValidator();

        var result = validator.Validate(new GetAllUsersRequest(1, 20));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_Should_Fail_When_Page_Is_Less_Than_One()
    {
        var validator = new GetAllUsersValidator();

        var result = validator.Validate(new GetAllUsersRequest(0, 20));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAllUsersRequest.Page));
    }

    [Fact]
    public void Validate_Should_Fail_When_PageSize_Is_Out_Of_Range()
    {
        var validator = new GetAllUsersValidator();

        var result = validator.Validate(new GetAllUsersRequest(1, 101));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(GetAllUsersRequest.PageSize));
    }
}