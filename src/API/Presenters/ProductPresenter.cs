using UserCrud.API.Dtos;
using UserCrud.API.Helpers;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Presenters;

public static class ProductPresenter
{
    public static ProductResponseDto ToHttp(Product product)
    {
        ArgumentNullException.ThrowIfNull(product.ProductCategory);
        
        return new ProductResponseDto()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PriceInCents = product.PriceInCents,
            ProductionTimeInDays = product.ProductionTimeInDays,
            ProductCategoryId = product.ProductCategoryId,
            Category = product.ProductCategory.Category,
        };
    }
    
    public static PaginationResponseDto<ProductResponseDto> ToHttp(IEnumerable<Product> products, int count, int page, int itemsPerPage)
    {
        var productsResponse = products.Select(ToHttp);
        
        return PaginationHelper.FormatResponse(productsResponse, count, page, itemsPerPage);
    }
}