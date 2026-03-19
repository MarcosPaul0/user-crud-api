using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.CreateProduct;

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