namespace GestaoDeUsuarios.Application.Abstractions.Data;

using Microsoft.EntityFrameworkCore;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
