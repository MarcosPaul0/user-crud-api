using UserCrud.Domain.Entities;

namespace UserCrud.Infrastructure.Repositories.FilterBuilders;

public class ProductFilterBuilder(IQueryable<Product> query) : BaseFilterBuilder<Product>(query)
{
    private IQueryable<Product> _query = query;

    protected override void Order()
    {
        _query = _query.OrderBy(product => product.CreatedAt);
    }
    
    public ProductFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            _query = _query.Where(product => product.Name == name);
        }

        return this;
    }
    
    public ProductFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            _query = _query.Where(product => product.IsActive == isActive);
        }

        return this;
    }

    public ProductFilterBuilder FilterByProductCategoryId(Guid? productCategoryId)
    {
        if (productCategoryId != null && productCategoryId != Guid.Empty)
        {
            _query = _query.Where(product => product.ProductCategoryId == productCategoryId);
        }

        return this;
    }
}