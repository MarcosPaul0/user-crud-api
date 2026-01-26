using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories;

public class PhoneRepository(ApplicationDbContext context) : IPhoneRepository
{
    public async Task<Phone> CreateAsync(Phone phone, CancellationToken cancellationToken = default)
    {
        await context.AddAsync(phone, cancellationToken);
        
        return phone;
    }

    public async Task<Phone> DeleteAsync(Phone phone)
    {
        context.Remove(phone);
        
        return await Task.FromResult(phone);
    }

    public async Task<IEnumerable<Phone>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Phone.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Phone?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await context.Phone.FindAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Phone>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Phone
            .Where(phone => phone.UserId == Guid.Parse(userId))
            .ToListAsync(cancellationToken);
    }

    public async Task<Phone> UpdateAsync(Phone phone)
    {
        context.Update(phone);
        
        return await Task.FromResult(phone);
    }
}