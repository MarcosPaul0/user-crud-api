using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProducts;

public class ListProductsUseCase(IProductRepository productRepository) : IListProductsUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductDto listProductDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product(
            listProductDto.Name,
            listProductDto.ProductCategoryId);

        var products = await productRepository.FindAllAsync(productFilter, listProductDto.Page,
            listProductDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await productRepository.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}