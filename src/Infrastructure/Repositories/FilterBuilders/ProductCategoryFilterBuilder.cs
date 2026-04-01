using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class ProductCategoryFilterBuilder(IQueryable<ProductCategory> query) : BaseFilterBuilder<ProductCategory>(query)
{
    protected override void Order()
    {
        _query = _query.OrderBy(productCategory => productCategory.CreatedAt);
    }

    public ProductCategoryFilterBuilder FilterByCategory(string? category)
    {
        if (!string.IsNullOrEmpty(category))
        {
            _query = _query.Where(productCategory => productCategory.Category.ToLower().Contains(category.ToLower()));
        }

        return this;
    }

    public ProductCategoryFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            _query = _query.Where(productCategory => productCategory.IsActive == isActive);
        }

        return this;
    }
}