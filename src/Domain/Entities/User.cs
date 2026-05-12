// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public UserRole Role { get; set; }

    public User(string name, string email, string password, UserRole role, DateTime createdAt)
    {
        this.Name = name;
        this.Email = email;
        this.Password = password;
        this.Role = role;
        this.CreatedAt = createdAt;
    }

    public User(string? name, UserRole? role)
    {
        if (!string.IsNullOrEmpty(name))
        {
            this.Name = name;
        }

        if (role != null)
        {
            this.Role = role.Value;
        }
    }
}