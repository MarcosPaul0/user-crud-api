// <copyright file="ProductCategoryFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class ProductCategoryFilterBuilder(IQueryable<ProductCategory> query) : BaseFilterBuilder<ProductCategory>(query)
{
    protected override void Order()
    {
        this.query = this.query.OrderBy(productCategory => productCategory.CreatedAt);
    }

    public ProductCategoryFilterBuilder FilterByCategory(string? category)
    {
        if (!string.IsNullOrEmpty(category))
        {
            this.query = this.query.Where(productCategory => productCategory.Category.ToLower().Contains(category.ToLower()));
        }

        return this;
    }

    public ProductCategoryFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.query = this.query.Where(productCategory => productCategory.IsActive == isActive);
        }

        return this;
    }
}