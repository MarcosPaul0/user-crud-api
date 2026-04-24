using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class PhoneRepository(ApplicationDbContext context) : BaseRepository<Phone>(context), IPhoneRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Phone>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Phone.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Phone>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Phone
            .Where(phone => phone.UserId == Guid.Parse(userId))
            .ToListAsync(cancellationToken);
    }
}