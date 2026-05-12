// <copyright file="BaseFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public abstract class BaseFilterBuilder<T>(IQueryable<T> query)
    where T : Entity
{
    public IQueryable<T> Build()
    {
        return this.Query;
    }

    public BaseFilterBuilder<T> ApplyPagination(int page, int itemsPerPage)
    {
        this.Order();

        var currentPage = page > 0 ? page - 1 : 0;

        this.Query = this.Query.Skip(currentPage * itemsPerPage).Take(itemsPerPage);

        return this;
    }

    protected IQueryable<T> Query { get; set; } = query;

    protected abstract void Order();
}
