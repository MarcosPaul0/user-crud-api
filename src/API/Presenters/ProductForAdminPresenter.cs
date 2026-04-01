using AutoriaStore.API.Dtos;
using AutoriaStore.API.Helpers;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.API.Presenters;

public static class ProductForAdminPresenter
{
    public static ProductForAdminResponseDto ToHttp(Product product)
    {
        ArgumentNullException.ThrowIfNull(product.ProductCategory);
        
        ArgumentNullException.ThrowIfNull(product.ProductImages);
        
        return new ProductForAdminResponseDto()
        {
            Id = product.Id,
            Name = product.Name,
            PrintDescription = product.PrintDescription,
            Description = product.Description,
            PriceInCents = product.PriceInCents,
            ProductionTimeInMinutes = product.ProductionTimeInMinutes,
            DiscountPercentage = product.DiscountPercentage,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            ProductCategoryId = product.ProductCategoryId,
            Category = product.ProductCategory.Category,
            ProductImages = ProductImagePresenter.ToHttp(product.ProductImages)
        };
    }
    
    public static PaginationResponseDto<ProductForAdminResponseDto> ToHttp(IEnumerable<Product> products, int count, int page, int itemsPerPage)
    {
        var productsResponse = products.Select(ToHttp);
        
        return PaginationHelper.FormatResponse(productsResponse, count, page, itemsPerPage);
    }
}