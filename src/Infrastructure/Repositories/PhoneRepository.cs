using Microsoft.EntityFrameworkCore;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;
using UserCrud.Infrastructure.Context;

namespace UserCrud.Infrastructure.Repositories
{
    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _phoneContext;

        public PhoneRepository(ApplicationDbContext context)
        {
            _phoneContext = context;
        }

        public async Task<Phone> Create(Phone phone)
        {
            _phoneContext.Add(phone);
            await _phoneContext.SaveChangesAsync();
            return phone;
        }

        public async Task<Phone> Delete(Phone phone)
        {
            _phoneContext.Remove(phone);
            await _phoneContext.SaveChangesAsync();
            return phone;
        }

        public async Task<IEnumerable<Phone>> FindAll()
        {
            return await _phoneContext.Phone.AsNoTracking().ToListAsync();
        }

        public async Task<Phone?> FindById(string id)
        {
            return await _phoneContext.Phone.FindAsync(id);
        }

        public async Task<IEnumerable<Phone>> FindByUserId(string userId)
        {
            return await _phoneContext.Phone
                .Where(phone => phone.UserId == Guid.Parse(userId))
                .ToListAsync();
        }

        public async Task<Phone> Update(Phone phone)
        {
            _phoneContext.Update(phone);
            await _phoneContext.SaveChangesAsync();
            return phone;
        }
    }
}