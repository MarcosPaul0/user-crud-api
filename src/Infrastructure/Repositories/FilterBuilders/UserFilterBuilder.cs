// <copyright file="UserFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class UserFilterBuilder(IQueryable<User> query) : BaseFilterBuilder<User>(query)
{
    protected override void Order()
    {
        this.query = this.query.OrderBy(user => user.CreatedAt);
    }

    public UserFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.query = this.query.Where(user => user.Name == name);
        }

        return this;
    }

    public UserFilterBuilder FilterByRole(UserRole? role)
    {
        if (role != null && role != UserRole.None)
        {
            this.query = this.query.Where(user => user.Role == role);
        }

        return this;
    }
}