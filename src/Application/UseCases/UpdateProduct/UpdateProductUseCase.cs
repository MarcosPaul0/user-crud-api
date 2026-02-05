using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.UpdateProduct;

public class UpdateProductUseCase(
    IProductRepository productRepository,
    IProductCategoryRepository productCategoryRepository,
    IUnitOfWork unitOfWork) : IUpdateProductUseCase
{
    public async Task ExecuteAsync(Guid productId, UpdateProductDto updateProductDto, CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }

        var isUpdated = false;
        
        if (updateProductDto.Name != null && updateProductDto.Name != product.Name)
        {
            var productAlreadyExists = await productRepository.FindByNameAsync(updateProductDto.Name, cancellationToken);

            if (productAlreadyExists != null)
            {
                throw new ConflictException(ExceptionMessages.PRODUCT_ALREADY_EXISTS);
            }
            
            product.Name = updateProductDto.Name;
            isUpdated = true;
        }
        
        if (updateProductDto.ProductCategoryId != null && updateProductDto.ProductCategoryId != product.ProductCategoryId)
        {
            var productCategory =
                await productCategoryRepository.FindByIdAsync(updateProductDto.ProductCategoryId.Value, cancellationToken);

            if (productCategory == null)
            {
                throw new NotFoundException(ExceptionMessages.PRODUCT_CATEGORY_NOT_FOUND);
            }
            
            product.ProductCategoryId = updateProductDto.ProductCategoryId.Value;
            isUpdated = true;
        }
        
        if (updateProductDto.Description != null && updateProductDto.Description != product.Description)
        {
            product.Description = updateProductDto.Description;
            isUpdated = true;
        }
        
        if (updateProductDto.PriceInCents != null && updateProductDto.PriceInCents != product.PriceInCents)
        {
            product.PriceInCents = updateProductDto.PriceInCents.Value;
            isUpdated = true;
        }
        
        if (updateProductDto.ProductionTimeInDays != null && updateProductDto.ProductionTimeInDays != product.ProductionTimeInDays)
        {
            product.ProductionTimeInDays = updateProductDto.ProductionTimeInDays.Value;
            isUpdated = true;
        }
        
        if (updateProductDto.ProductionTimeInDays != null && updateProductDto.ProductionTimeInDays != product.ProductionTimeInDays)
        {
            product.ProductionTimeInDays = updateProductDto.ProductionTimeInDays.Value;
            isUpdated = true;
        }
        
        if (updateProductDto.StockQuantity != null && updateProductDto.StockQuantity != product.StockQuantity)
        {
            product.StockQuantity = updateProductDto.StockQuantity.Value;
            isUpdated = true;
        }
        
        if (updateProductDto.IsActive != null && updateProductDto.IsActive != product.IsActive)
        {
            product.IsActive = updateProductDto.IsActive.Value;
            isUpdated = true;
        }
        
        if (isUpdated)
        {
            product.UpdatedAt = DateTime.UtcNow;
            
            await productRepository.UpdateAsync(product, cancellationToken);
            
            await unitOfWork.SaveChangesAsync();
        }
    }
}