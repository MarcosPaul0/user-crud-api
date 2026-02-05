using UserCrud.Application.Dtos;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProductsByAdmin;

public class ListProductsByAdminUseCase(IProductRepository productRepository) : IListProductsByAdminUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductByAdminDto listProductByAdminDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product(
            listProductByAdminDto.Name,
            listProductByAdminDto.ProductCategoryId,
            listProductByAdminDto.IsActive);

        var products = await productRepository.FindAllAsync(productFilter, listProductByAdminDto.Page,
            listProductByAdminDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await productRepository.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}