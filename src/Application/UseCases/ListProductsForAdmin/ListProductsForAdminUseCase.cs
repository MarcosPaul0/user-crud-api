using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.ListProductsForAdmin;

public sealed class ListProductsForAdminUseCase(IUnitOfWork unitOfWork) : IListProductsForAdminUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsByAdminDto listProductsByAdminDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product(
            listProductsByAdminDto.Name,
            listProductsByAdminDto.ProductCategoryId,
            listProductsByAdminDto.IsActive);

        var products = await unitOfWork.Product.FindAllAsync(productFilter, listProductsByAdminDto.Page,
            listProductsByAdminDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await unitOfWork.Product.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}