using GestaoDeUsuarios.Application.Abstractions.Data;
using GestaoDeUsuarios.Domain.Entities;
using GestaoDeUsuarios.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GestaoDeUsuarios.Api.IntegrationTests;

public sealed class ApiWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"api-integration-tests-{Guid.NewGuid()}";
    private readonly string? _originalEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    private readonly string? _originalConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__gestao-de-usuarios");

    public ApiWebApplicationFactory()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        Environment.SetEnvironmentVariable(
            "ConnectionStrings__gestao-de-usuarios",
            "Host=localhost;Database=gestao_test;Username=test;Password=test");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        builder.ConfigureAppConfiguration((_, configurationBuilder) =>
        {
            configurationBuilder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:gestao-de-usuarios"] = "Host=localhost;Database=gestao_test;Username=test;Password=test"
            });
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IDbContextOptionsConfiguration<AppDbContext>>();
            services.RemoveAll<DbContextOptions>();
            services.RemoveAll<DbContextOptions<AppDbContext>>();
            services.RemoveAll<AppDbContext>();
            services.RemoveAll<IAppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            services.AddScoped<IAppDbContext>(serviceProvider => serviceProvider.GetRequiredService<AppDbContext>());

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                    options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.SchemeName, _ => { });
        });
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", _originalEnvironment);
            Environment.SetEnvironmentVariable("ConnectionStrings__gestao-de-usuarios", _originalConnectionString);
        }

        base.Dispose(disposing);
    }

    public async Task<ApplicationUser> CriarUsuarioAsync(string nome, string email, bool ativo = true)
    {
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
        var partes = nome.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        var user = new ApplicationUser
        {
            FirstName = partes[0],
            LastName = partes.Length > 1 ? partes[1] : string.Empty,
            Email = email,
            UserName = email,
            IsActive = ativo
        };

        var result = await userManager.CreateAsync(user, "Senha@123");

        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(error => error.Description)));

        return user;
    }
}