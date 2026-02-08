using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProductsByAdmin;

public class ListProductsByAdminUseCase(IProductRepository productRepository) : IListProductsByAdminUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsByAdminDto listProductsByAdminDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product(
            listProductsByAdminDto.Name,
            listProductsByAdminDto.ProductCategoryId,
            listProductsByAdminDto.IsActive);

        var products = await productRepository.FindAllAsync(productFilter, listProductsByAdminDto.Page,
            listProductsByAdminDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await productRepository.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}