// <copyright file="ProductFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class ProductFilterBuilder(IQueryable<Product> query) : BaseFilterBuilder<Product>(query)
{
    protected override void Order()
    {
        this.query = this.query.OrderBy(product => product.CreatedAt);
    }

    public ProductFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.query = this.query.Where(product => product.Name == name);
        }

        return this;
    }

    public ProductFilterBuilder FilterByIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.query = this.query.Where(product => product.IsActive == isActive);
        }

        return this;
    }

    public ProductFilterBuilder FilterByProductCategoryIsActive(bool? isActive)
    {
        if (isActive != null)
        {
            this.query = this.query.Where(product => product.ProductCategory.IsActive == isActive);
        }

        return this;
    }

    public ProductFilterBuilder FilterByProductCategoryId(Guid? productCategoryId)
    {
        if (productCategoryId != null && productCategoryId != Guid.Empty)
        {
            this.query = this.query.Where(product => product.ProductCategoryId == productCategoryId);
        }

        return this;
    }
}