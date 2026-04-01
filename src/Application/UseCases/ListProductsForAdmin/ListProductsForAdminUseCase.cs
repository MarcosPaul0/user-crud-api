using AutoriaStore.Application.Dtos;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.Application.UseCases.ListProductsForAdmin;

public sealed class ListProductsForAdminUseCase(IUnitOfWork unitOfWork) : IListProductsForAdminUseCase
{
    public async Task<(IEnumerable<Product> products, int count)> ExecuteAsync(
        ListProductsByAdminDto listProductsByAdminDto, 
        CancellationToken cancellationToken)
    {
        var productFilter = new Product();
        
        if (listProductsByAdminDto.IsActive != null)
        {
            productFilter.IsActive = listProductsByAdminDto.IsActive.Value;
        }
        
        if (!string.IsNullOrWhiteSpace(listProductsByAdminDto.Name))
        {
            productFilter.Name = listProductsByAdminDto.Name;
        }
        
        if (listProductsByAdminDto.ProductCategoryId != null && listProductsByAdminDto.ProductCategoryId != Guid.Empty)
        {
            productFilter.ProductCategoryId = listProductsByAdminDto.ProductCategoryId.Value;
        }

        var products = await unitOfWork.Product.FindAllAsync(productFilter, listProductsByAdminDto.Page,
            listProductsByAdminDto.ItemsPerPage, cancellationToken);
        
        var productsCount = await unitOfWork.Product.CountAsync(productFilter, cancellationToken);
        
        return (products, productsCount);
    }
}