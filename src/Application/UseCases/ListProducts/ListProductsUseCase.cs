using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProducts;

public sealed class ListProductsUseCase(IUnitOfWork unitOfWork) : IListProductsUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsDto listProductsDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product(
            listProductsDto.Name,
            listProductsDto.ProductCategoryId);

        var products = await unitOfWork.Product.FindAllAsync(productFilter, listProductsDto.Page,
            listProductsDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await unitOfWork.Product.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}