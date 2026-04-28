using GestaoDeUsuarios.Application.Features.Users.Update;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class UpdateUserHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Validation_When_Request_Is_Invalid()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new UpdateUserHandler(userManager, new UpdateUserValidator());

        var result = await handler.HandleAsync(
            new UpdateUserRequest(Guid.NewGuid().ToString(), string.Empty, "invalid-email"),
            TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.InvalidData", result.Error?.Code);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new UpdateUserHandler(userManager, new UpdateUserValidator());

        var result = await handler.HandleAsync(
            new UpdateUserRequest(Guid.NewGuid().ToString(), "Jane Doe", "jane@cwm.dev"),
            TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error?.Code);
        Assert.Equal(ErrorType.NotFound, result.Error?.Type);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Conflict_When_Email_Belongs_To_Another_User()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);

        var firstUser = new ApplicationUser
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@cwm.dev",
            UserName = "jane@cwm.dev"
        };
        var secondUser = new ApplicationUser
        {
            FirstName = "John",
            LastName = "Smith",
            Email = "john@cwm.dev",
            UserName = "john@cwm.dev"
        };

        await userManager.CreateAsync(firstUser, "Valid@123");
        await userManager.CreateAsync(secondUser, "Valid@123");
        var handler = new UpdateUserHandler(userManager, new UpdateUserValidator());

        var result = await handler.HandleAsync(
            new UpdateUserRequest(firstUser.Id, "Jane Doe", secondUser.Email!),
            TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.EmailTaken", result.Error?.Code);
        Assert.Equal(ErrorType.Conflict, result.Error?.Type);
    }

    [Fact]
    public async Task HandleAsync_Should_Update_User_When_Request_Is_Valid()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var user = new ApplicationUser
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@cwm.dev",
            UserName = "jane@cwm.dev"
        };
        await userManager.CreateAsync(user, "Valid@123");
        var handler = new UpdateUserHandler(userManager, new UpdateUserValidator());

        var result = await handler.HandleAsync(
            new UpdateUserRequest(user.Id, "Janet Dorian", "janet@cwm.dev"),
            TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);

        var updatedUser = await userManager.FindByIdAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.Equal("Janet", updatedUser!.FirstName);
        Assert.Equal("Dorian", updatedUser.LastName);
        Assert.Equal("janet@cwm.dev", updatedUser.Email);
        Assert.Equal("janet@cwm.dev", updatedUser.UserName);
    }
}