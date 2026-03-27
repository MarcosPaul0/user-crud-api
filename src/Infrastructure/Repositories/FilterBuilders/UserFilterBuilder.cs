using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class UserFilterBuilder(IQueryable<User> query) : BaseFilterBuilder<User>(query)
{
    protected override void Order()
    {
        _query = _query.OrderBy(user => user.CreatedAt);
    }
    
    public UserFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _query = _query.Where(user => user.Name == name);
        }

        return this;
    }
    
    public UserFilterBuilder FilterByRole(UserRole? role)
    {
        if (role != null && role != UserRole.None)
        {
            _query = _query.Where(user => user.Role == role);
        }

        return this;
    }
}