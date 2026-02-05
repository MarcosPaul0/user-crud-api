using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class BaseRepository<T>(ApplicationDbContext context) where T : Entity
{
    public virtual async Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default) 
    {
        var result = await context.Set<T>().AddAsync(entity, cancellationToken);         

        return result.Entity;
    }
    
    public virtual async Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.Set<T>().FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        return result;
    }
    
    public virtual async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        context.Set<T>().Update(entity);

        return entity;
    }
    
    public virtual async Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        var result = context.Set<T>().Remove(entity);            

        return result.Entity;
    }
}