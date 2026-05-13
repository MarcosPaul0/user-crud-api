// <copyright file="OrderRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace AutoriaStore.Infrastructure.Repositories;

public class OrderRepository(ApplicationDbContext context) : BaseRepository<Order>(context), IOrderRepository
{
    public async Task<Order?> FindByPaymentIdAsync(string paymentId, CancellationToken cancellationToken = default)
    {
        return await context.Order
            .Include(order => order.ProductOrders)
            .FirstOrDefaultAsync(order => order.PaymentId == paymentId, cancellationToken);
    }

    public async Task<Order?> FindByUserIdAndOrderIdAsync(
        Guid userId,
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return await context.Order
            .Include(order => order.ProductOrders)
            .FirstOrDefaultAsync(
                order => order.Id == orderId && order.UserId == userId,
                cancellationToken);
    }
}
