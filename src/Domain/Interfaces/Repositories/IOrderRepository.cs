// <copyright file="IOrderRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Domain.Interfaces.Repositories;

public interface IOrderRepository : IBaseRepository<Order>
{
    Task<Order?> FindByPaymentIdAsync(string paymentId, CancellationToken cancellationToken = default);

    Task<Order?> FindByUserIdAndOrderIdAsync(Guid userId, Guid orderId, CancellationToken cancellationToken = default);
}
