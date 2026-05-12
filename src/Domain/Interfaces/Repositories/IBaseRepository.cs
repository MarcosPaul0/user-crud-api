// <copyright file="IBaseRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);

    Task<T?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<T> UpdateAsync(T entity, CancellationToken cancellationToken = default);

    Task<T> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}