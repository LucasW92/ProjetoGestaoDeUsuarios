using GestaoDeUsuarios.Application.Features.Users.Create;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class CreateUserHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Create_User_When_Request_Is_Valid()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new CreateUserHandler(userManager);
        var request = new CreateUserRequest("Jane Doe", "jane@cwm.dev", "Valid@123", "Valid@123");

        var result = await handler.HandleAsync(request, TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Jane Doe", result.Value!.Nome);
        Assert.Equal("jane@cwm.dev", result.Value.Email);
        Assert.True(result.Value.Ativo);

        var createdUser = await userManager.FindByEmailAsync(request.Email);
        Assert.NotNull(createdUser);
        Assert.Equal("Jane", createdUser!.FirstName);
        Assert.Equal("Doe", createdUser.LastName);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Conflict_When_Email_Already_Exists()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        await userManager.CreateAsync(
            new ApplicationUser
            {
                FirstName = "Existing",
                LastName = "User",
                Email = "existing@cwm.dev",
                UserName = "existing@cwm.dev"
            },
            "Valid@123");

        var handler = new CreateUserHandler(userManager);
        var request = new CreateUserRequest("Jane Doe", "existing@cwm.dev", "Valid@123", "Valid@123");

        var result = await handler.HandleAsync(request, TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.EmailTaken", result.Error?.Code);
        Assert.Equal(ErrorType.Conflict, result.Error?.Type);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Validation_When_Identity_Creation_Fails()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new CreateUserHandler(userManager);
        var request = new CreateUserRequest("Jane Doe", "jane@cwm.dev", "short", "short");

        var result = await handler.HandleAsync(request, TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.CreationFailed", result.Error?.Code);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }
}