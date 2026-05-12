// <copyright file="Phone.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace AutoriaStore.Domain.Entities;

public class Phone : Entity
{
    public string? PhoneNumber { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;

    public Phone(string phoneNumber)
    {
        this.PhoneNumber = phoneNumber;
    }
}