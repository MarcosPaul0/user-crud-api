using UserCrud.API.Dtos;
using UserCrud.API.Helpers;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Presenters;

public static class ProductByAdminPresenter
{
    public static ProductByAdminResponseDto ToHttp(Product product)
    {
        ArgumentNullException.ThrowIfNull(product.ProductCategory);
        
        return new ProductByAdminResponseDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PriceInCents = product.PriceInCents,
            ProductionTimeInDays = product.ProductionTimeInDays,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt,
            UpdatedAt = product.UpdatedAt,
            ProductCategoryId = product.ProductCategoryId,
            Category = product.ProductCategory.Category,
        };
    }
    
    public static PaginationResponseDto<ProductByAdminResponseDto> ToHttp(IEnumerable<Product> products, int count, int page, int itemsPerPage)
    {
        var productsResponse = products.Select(ToHttp);
        
        return PaginationHelper.FormatResponse(productsResponse, count, page, itemsPerPage);
    }
}