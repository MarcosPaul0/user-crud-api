using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.Application.UseCases.CreateProduct;

public sealed class CreateProductUseCase(IUnitOfWork unitOfWork) : ICreateProductUseCase
{
    public async Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        var productCategory =
            await unitOfWork.ProductCategory.FindByIdAsync(createProductDto.ProductCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        var productAlreadyExists = await unitOfWork.Product.FindByNameAsync(createProductDto.Name, cancellationToken);

        if (productAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_ALREADY_EXISTS);
        }

        var newProduct = new Product(
            createProductDto.Name,
            createProductDto.Description,
            createProductDto.PrintDescription,
            createProductDto.PriceInCents,
            createProductDto.ProductionTimeInMinutes,
            createProductDto.DiscountPercentage,
            createProductDto.StockQuantity,
            createProductDto.ProductCategoryId,
            DateTime.UtcNow);

        await unitOfWork.Product.CreateAsync(newProduct, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}