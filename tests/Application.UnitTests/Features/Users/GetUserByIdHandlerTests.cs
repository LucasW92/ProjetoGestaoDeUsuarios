using GestaoDeUsuarios.Application.Features.Users.GetById;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class GetUserByIdHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_User_When_Found()
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
        var handler = new GetUserByIdHandler(userManager);

        var result = await handler.HandleAsync(new GetUserByIdRequest(user.Id), TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(user.Id, result.Value!.Id);
        Assert.Equal("Jane Doe", result.Value.Nome);
        Assert.True(result.Value.Ativo);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_NotFound_When_User_Does_Not_Exist()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new GetUserByIdHandler(userManager);

        var result = await handler.HandleAsync(new GetUserByIdRequest(Guid.NewGuid().ToString()), TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("Users.NotFound", result.Error?.Code);
        Assert.Equal(ErrorType.NotFound, result.Error?.Type);
    }
}