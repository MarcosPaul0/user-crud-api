// <copyright file="UserFilterBuilder.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Infrastructure.Repositories.FilterBuilders;

public class UserFilterBuilder(IQueryable<User> query) : BaseFilterBuilder<User>(query)
{
    public UserFilterBuilder FilterByName(string? name)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.Query = this.Query.Where(user => user.Name == name);
        }

        return this;
    }

    public UserFilterBuilder FilterByRole(UserRole? role)
    {
        if (role != null && role != UserRole.None)
        {
            this.Query = this.Query.Where(user => user.Role == role);
        }

        return this;
    }

    protected override void Order()
    {
        this.Query = this.Query.OrderBy(user => user.CreatedAt);
    }
}
