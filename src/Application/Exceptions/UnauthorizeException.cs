// <copyright file="UnauthorizeException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Exceptions;

public class UnauthorizeException(string message) : Exception(message)
{
}