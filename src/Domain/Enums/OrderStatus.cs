// <copyright file="OrderStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Enums;

public enum OrderStatus
{
    AwaitingPayment = 1,
    Paid = 2,
    PaymentExpired = 3,
    PaymentCancelled = 4,
    Refunded = 5,
    Disputed = 6,
    Lost = 7,
}
