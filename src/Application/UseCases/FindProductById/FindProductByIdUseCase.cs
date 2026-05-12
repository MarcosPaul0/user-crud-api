// <copyright file="FindProductByIdUseCase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces.Repositories;

namespace AutoriaStore.Application.UseCases.FindProductById;

public sealed class FindProductByIdUseCase(IUnitOfWork unitOfWork) : IFindProductByIdUseCase
{
    public async Task<Product> ExecuteAsync(Guid productId, CancellationToken cancellationToken)
    {
        var product = await unitOfWork.Product.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCTNOTFOUND);
        }

        return product;
    }
}