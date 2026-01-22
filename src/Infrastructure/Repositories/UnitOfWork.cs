using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}