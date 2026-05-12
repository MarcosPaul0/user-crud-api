// <copyright file="BaseFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public abstract class BaseFilterBuilder<T>(IQueryable<T> query)
    where T : Entity
{
    protected IQueryable<T> query = query;

    protected abstract void Order();

    public IQueryable<T> Build()
    {
        return this.query;
    }

    public BaseFilterBuilder<T> ApplyPagination(int page, int itemsPerPage)
    {
        this.Order();

        var currentPage = page > 0 ? page - 1 : 0;

        this.query = this.query.Skip(currentPage * itemsPerPage).Take(itemsPerPage);

        return this;
    }
}