using GestaoDeUsuarios.Application.Features.Users.GetAll;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;

namespace GestaoDeUsuarios.Application.UnitTests.Features.Users;

public sealed class GetAllUsersHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Return_Paged_Users_Ordered_By_First_And_Last_Name()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);

        await userManager.CreateAsync(new ApplicationUser
        {
            FirstName = "Charlie",
            LastName = "Zulu",
            Email = "charlie@cwm.dev",
            UserName = "charlie@cwm.dev"
        }, "Valid@123");
        await userManager.CreateAsync(new ApplicationUser
        {
            FirstName = "Alice",
            LastName = "Zulu",
            Email = "alice.zulu@cwm.dev",
            UserName = "alice.zulu@cwm.dev"
        }, "Valid@123");
        await userManager.CreateAsync(new ApplicationUser
        {
            FirstName = "Alice",
            LastName = "Bravo",
            Email = "alice.bravo@cwm.dev",
            UserName = "alice.bravo@cwm.dev"
        }, "Valid@123");

        var handler = new GetAllUsersHandler(userManager);

        var result = await handler.HandleAsync(new GetAllUsersRequest(1, 2), TestContext.Current.CancellationToken);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(3, result.Value!.TotalCount);
        Assert.Equal(2, result.Value.Items.Count);
        Assert.Equal("Alice Bravo", result.Value.Items[0].Nome);
        Assert.Equal("Alice Zulu", result.Value.Items[1].Nome);
    }

    [Fact]
    public async Task HandleAsync_Should_Return_Validation_When_Pagination_Is_Invalid()
    {
        await using var dbContext = TestIdentityFactory.CreateDbContext();
        using var userManager = TestIdentityFactory.CreateUserManager(dbContext);
        var handler = new GetAllUsersHandler(userManager);

        var result = await handler.HandleAsync(new GetAllUsersRequest(0, 101), TestContext.Current.CancellationToken);

        Assert.True(result.IsFailure);
        Assert.Equal("400", result.Error?.Code);
        Assert.Equal(ErrorType.Validation, result.Error?.Type);
    }
}