// <copyright file="OrderProductRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;

namespace AutoriaStore.Infrastructure.Repositories;

public class OrderProductRepository(ApplicationDbContext context) : BaseRepository<OrderProduct>(context), IOrderProductRepository
{
}