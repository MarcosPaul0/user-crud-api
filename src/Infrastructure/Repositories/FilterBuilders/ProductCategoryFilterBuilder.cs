// <copyright file="ProductCategoryFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class ProductCategoryFilterBuilder(IQueryable<ProductCategory> query) : BaseFilterBuilder<ProductCategory>(query)
{
    public ProductCategoryFilterBuilder FilterByCategory(string? category)
    {
        if (!string.IsNullOrEmpty(category))
        {
            this.Query = this.Query.Where(productCategory => productCategory.Category.ToLower().Contains(category.ToLower()));
        }

        return this;
    }

    public ProductCategoryFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.Query = this.Query.Where(productCategory => productCategory.IsActive == isActive);
        }

        return this;
    }

    protected override void Order()
    {
        this.Query = this.Query.OrderBy(productCategory => productCategory.CreatedAt);
    }
}
