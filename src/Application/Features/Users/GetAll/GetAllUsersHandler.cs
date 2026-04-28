using GestaoDeUsuarios.Application.Common;
using GestaoDeUsuarios.Domain.Common;
using GestaoDeUsuarios.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeUsuarios.Application.Features.Users.GetAll;

public sealed class GetAllUsersHandler(UserManager<ApplicationUser> userManager)
{
    public async Task<Result<PagedResult<UserResponse>>> HandleAsync(
        GetAllUsersRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Page < 1 || request.PageSize < 1 || request.PageSize > 100)
        {
            return Result.Failure<PagedResult<UserResponse>>(Error.Validation("400", "Parâmetros de paginação inválidos. A página deve ser maior que 0 e o tamanho da página deve estar entre 1 e 100."));
        }

        var page = request.Page;
        var pageSize = request.PageSize;

        var query = userManager.Users
            .AsNoTracking()
            .OrderBy(user => user.FirstName)
            .ThenBy(user => user.LastName);

        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(user => new UserResponse(
            user.Id,
            NomeUsuarioMapper.Juntar(user.FirstName, user.LastName),
            user.Email!,
            user.IsActive,
            user.CreatedAt,
            user.UpdatedAt
            )).ToListAsync(cancellationToken);

        return Result.Success(new PagedResult<UserResponse>(
            users,
            totalCount,
            page,
            pageSize
            ));
    }
}

