// <copyright file="ProductFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class ProductFilterBuilder(IQueryable<Product> query) : BaseFilterBuilder<Product>(query)
{
    public ProductFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.Query = this.Query.Where(product => product.Name == name);
        }

        return this;
    }

    public ProductFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.Query = this.Query.Where(product => product.IsActive == isActive);
        }

        return this;
    }

    public ProductFilterBuilder FilterByProductCategoryIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.Query = this.Query.Where(product => product.ProductCategory.IsActive == isActive);
        }

        return this;
    }

    public ProductFilterBuilder FilterByProductCategoryId(Guid? productCategoryId)
    {
        if (productCategoryId != null && productCategoryId != Guid.Empty)
        {
            this.Query = this.Query.Where(product => product.ProductCategoryId == productCategoryId);
        }

        return this;
    }

    protected override void Order()
    {
        this.Query = this.Query.OrderBy(product => product.CreatedAt);
    }
}
