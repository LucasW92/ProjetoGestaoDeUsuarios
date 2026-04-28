using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace GestaoDeUsuarios.Api.IntegrationTests;

public sealed class UsersEndpointsIntegrationTests(ApiWebApplicationFactory factory) : IClassFixture<ApiWebApplicationFactory>
{
    [Fact]
    public async Task GetUserById_Should_Return_Envelope_Com_Contrato_Do_Usuario()
    {
        var user = await factory.CriarUsuarioAsync("Maria Silva", "maria.silva@empresa.com");
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/users/{user.Id}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        var root = json.RootElement;
        var dados = root.GetProperty("dados_resposta");

        Assert.Equal("Maria Silva", dados.GetProperty("nome").GetString());
        Assert.Equal("maria.silva@empresa.com", dados.GetProperty("email").GetString());
        Assert.True(dados.GetProperty("ativo").GetBoolean());
        Assert.Matches("^\\d{2}/\\d{2}/\\d{4} \\d{2}:\\d{2}:\\d{2}$", dados.GetProperty("criado_em").GetString()!);
        Assert.Matches("^\\d+ ms$", root.GetProperty("tempo_da_resposta").GetString()!);
        Assert.True(root.TryGetProperty("timestamp_resposta", out _));
        Assert.False(dados.TryGetProperty("updated_at", out _));
        Assert.False(dados.TryGetProperty("first_name", out _));
        Assert.False(dados.TryGetProperty("last_name", out _));
    }

    [Fact]
    public async Task Register_Should_Aceitar_Nome_Como_Entrada_E_Retornar_Envelope_Padronizado()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/identity/register", new
        {
            nome = "Carlos Alberto",
            email = "carlos.alberto@empresa.com",
            password = "Senha@123",
            confirm_password = "Senha@123"
        }, TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken));
        var dados = json.RootElement.GetProperty("dados_resposta");

        Assert.Equal("Usuário cadastrado com sucesso.", dados.GetProperty("mensagem").GetString());

        using var scope = factory.Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
        var createdUser = await userManager.FindByEmailAsync("carlos.alberto@empresa.com");

        Assert.NotNull(createdUser);
        Assert.Equal("Carlos", createdUser!.FirstName);
        Assert.Equal("Alberto", createdUser.LastName);
    }

    [Fact]
    public async Task GetUserById_Should_Return_Cors_Header_Para_Localhost()
    {
        var user = await factory.CriarUsuarioAsync("Ana Clara", "ana.clara@empresa.com");
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Origin", "http://localhost:3000");

        var response = await client.GetAsync($"/api/users/{user.Id}", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(response.Headers.TryGetValues("Access-Control-Allow-Origin", out var values));
        Assert.Equal("http://localhost:3000", values!.Single());
    }
}