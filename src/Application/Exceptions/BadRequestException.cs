// <copyright file="BadRequestException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Exceptions;

public sealed class BadRequestException(string message) : Exception(message)
{
}
