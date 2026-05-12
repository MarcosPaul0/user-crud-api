// <copyright file="UserRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using AutoriaStore.Infrastructure.Repositories.FilterBuilders;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository
{
    private readonly ApplicationDbContext context = context;

    public async Task<IEnumerable<User>> FindAllAsync(
        User filter,
        int page,
        int itemsPerPage,
        CancellationToken cancellationToken = default)
    {
        var query = this.context.User.AsNoTracking();

        query = new UserFilterBuilder(query)
            .FilterByName(filter.Name)
            .FilterByRole(filter.Role)
            .ApplyPagination(page, itemsPerPage)
            .Build();

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(User filter, CancellationToken cancellationToken = default)
    {
        var query = this.context.User.AsNoTracking();

        query = new UserFilterBuilder(query)
            .FilterByName(filter.Name)
            .FilterByRole(filter.Role)
            .Build();

        return await query.CountAsync(cancellationToken);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await this.context.User.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task<User?> FindWithPonesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await this.context.User.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }
}