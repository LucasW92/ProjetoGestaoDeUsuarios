using GestaoDeUsuarios.Api.Extensions;
using GestaoDeUsuarios.Application.Features.Users.Create;
using GestaoDeUsuarios.Application.Features.Users.Deactivate;
using GestaoDeUsuarios.Application.Features.Users.GetAll;
using GestaoDeUsuarios.Application.Features.Users.GetById;
using GestaoDeUsuarios.Application.Features.Users.Update;

namespace GestaoDeUsuarios.Api.Endpoints;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users")
            .WithTags("Usuários")
            .RequireAuthorization(policy => policy.RequireRole("Admin"));

        group.MapPost("/", Create)
            .AddEndpointFilter<ValidationFilter<CreateUserRequest>>()
            .WithName("CreateUser")
            .WithSummary("Criar um novo usuário")
            .WithDescription("Cria um usuário de forma administrativa. Requer um usuário autenticado com perfil de administrador.");

        group.MapGet("/{id}", GetById)
            .WithName("GetUserById")
            .WithSummary("Obter um usuário por ID");

        group.MapGet("/", GetAll)
            .WithName("GetAllUsers")
            .WithSummary("Listar usuários de forma paginada");

        group.MapPut("/{id}", Update)
            .WithName("UpdateUser")
            .WithSummary("Atualizar os dados básicos de um usuário");

        group.MapPatch("/{id}/deactivate", Deactivate)
            .WithName("DeactivateUser")
            .WithSummary("Desativar usuário");

    }

    private static async Task<IResult> Create(CreateUserRequest request, CreateUserHandler handler, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(request, cancellationToken);
        return result.IsSuccess
            ? ApiHttpResults.Created(httpContext, $"/api/users/{result.Value!.Id}", result.Value)
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> GetById(string id, GetUserByIdHandler handler, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetUserByIdRequest(id), cancellationToken);

        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, result.Value)
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> GetAll(int? page, int? pageSize, GetAllUsersHandler handler, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var request = new GetAllUsersRequest
        (
            page ?? 1,
            pageSize ?? 10
        );

        var result = await handler.HandleAsync(request, cancellationToken);

        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, result.Value)
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> Update(string id, UpdateUserBody body, UpdateUserHandler handler, HttpContext httpContext, CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new UpdateUserRequest(
                id,
                body.Nome,
                body.Email),
            cancellationToken);

        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, new { Mensagem = "Usuário atualizado com sucesso." })
            : result.ToProblemDetails(httpContext);
    }

    private static async Task<IResult> Deactivate(string id, DeactivateUserHandler handler, HttpContext httpContext,
    CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeactivateUserRequest(id), cancellationToken);

        return result.IsSuccess
            ? ApiHttpResults.Ok(httpContext, new { Mensagem = "Usuário desativado com sucesso." })
            : result.ToProblemDetails(httpContext);
    }

}
