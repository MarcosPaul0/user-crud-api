// <copyright file="IPhoneRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IPhoneRepository : IBaseRepository<Phone>
{
    Task<IEnumerable<Phone>> FindAllAsync(CancellationToken cancellationToken = default);

    Task<IEnumerable<Phone>> FindByUserIdAsync(string userId, CancellationToken cancellationToken = default);
}