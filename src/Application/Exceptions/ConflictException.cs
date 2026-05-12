// <copyright file="ConflictException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Exceptions;

public class ConflictException(string message) : Exception(message)
{
}