using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.Repositories.FilterBuilders;

public abstract class BaseFilterBuilder<T>(IQueryable<T> query) where T : Entity 
{
    private IQueryable<T> _query = query;
    protected abstract void Order();

    public IQueryable<T> Build()
    {
        return _query;
    }
    
    public BaseFilterBuilder<T> ApplyPagination(int page, int itemsPerPage)
    {
        Order();

        var currentPage = page > 0 ? page - 1 : 0;
        
        _query = _query.Skip(currentPage * itemsPerPage).Take(itemsPerPage);

        return this;
    }
}