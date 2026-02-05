using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.CreateProduct;

public class CreateProductUseCase(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IUnitOfWork unitOfWork) : ICreateProductUseCase
{
    public async Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
    {
        var productCategory =
            await productCategoryRepository.FindByIdAsync(createProductDto.ProductCategoryId, cancellationToken);

        if (productCategory == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
        }
        
        var productAlreadyExists = await productRepository.FindByNameAsync(createProductDto.Name, cancellationToken);

        if (productAlreadyExists != null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_ALREADY_EXISTS);
        }

        var newProduct = new Product(
            createProductDto.Name,
            createProductDto.Description,
            createProductDto.PriceInCents,
            createProductDto.ProductionTimeInDays,
            createProductDto.StockQuantity,
            createProductDto.ProductCategoryId,
            DateTime.UtcNow);

        await productRepository.CreateAsync(newProduct, cancellationToken);
        
        await unitOfWork.SaveChangesAsync();
    }
}