using GestaoDeUsuarios.Application.Features.Users.Deactivate;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class DeactivateUserHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new DeactivateUserHandler(userManager);

        var result = await handler.HandleAsync(
            new DeactivateUserRequest(Guid.NewGuid().ToString()),
            TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error?.Code);
        Assert.Equal(ErrorType.NotFound, result.Error?.Type);
    }

    [Fact]
    public async Task HandleAsync_Should_Deactivate_Active_User()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var user = new ApplicationUser
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@cwm.dev",
            UserName = "jane@cwm.dev",
            IsActive = true
        };
        await userManager.CreateAsync(user, "Valid@123");
        var handler = new DeactivateUserHandler(userManager);

        var result = await handler.HandleAsync(new DeactivateUserRequest(user.Id), TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);

        var updatedUser = await userManager.FindByIdAsync(user.Id);
        Assert.NotNull(updatedUser);
        Assert.False(updatedUser!.IsActive);
        Assert.NotNull(updatedUser.DeactivatedAt);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Success_When_User_Is_Already_Inactive()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var user = new ApplicationUser
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane@cwm.dev",
            UserName = "jane@cwm.dev",
            IsActive = false,
            DeactivatedAt = DateTimeOffset.UtcNow.AddDays(-1)
        };
        await userManager.CreateAsync(user, "Valid@123");
        var handler = new DeactivateUserHandler(userManager);

        var result = await handler.HandleAsync(new DeactivateUserRequest(user.Id), TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);

        var unchangedUser = await userManager.FindByIdAsync(user.Id);
        Assert.NotNull(unchangedUser);
        Assert.False(unchangedUser!.IsActive);
        Assert.NotNull(unchangedUser.DeactivatedAt);
    }
}