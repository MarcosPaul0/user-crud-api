using UserCrud.API.Dtos;
using UserCrud.Domain.Entities;

namespace UserCrud.API.Presenters;

public static class ProductCategoryPresenter
{
    public static ProductCategoryResponseDto ToHttp(ProductCategory productCategory)
    {
        
        return new ProductCategoryResponseDto()
        {
            Id = productCategory.Id,
            Category = productCategory.Category,
            ProductCount = productCategory.ProductCount,
            CreatedAt = productCategory.CreatedAt
        };
    }
    
    public static PaginationResponseDto<ProductCategoryResponseDto> ToHttp(IEnumerable<ProductCategory> products, int count)
    {
        var productCategoriesResponse = products.Select(ToHttp);
        
        return new PaginationResponseDto<ProductCategoryResponseDto>()
        {
            HasNext = false,
            HasPrevious = false,
            Items = productCategoriesResponse,
            ItemsPerPage = count,
            TotalItems = count,
            Page = 1,
            TotalPages = 1,
        };
    }
}