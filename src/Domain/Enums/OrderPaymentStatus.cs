// <copyright file="OrderPaymentStatus.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Domain.Enums;

public enum OrderPaymentStatus
{
    Pending = 1,
    Paid = 2,
    Expired = 3,
    Cancelled = 4,
    Refunded = 5,
    Disputed = 6,
    Lost = 7,
}
