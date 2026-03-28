using AutoriaStore.API.Dtos;
using AutoriaStore.API.Helpers;
using AutoriaStore.Domain.Entities;

namespace AutoriaStore.API.Presenters;

public static class ProductPresenter
{
    public static ProductByIdResponseDto ToHttp(Product product)
    {
        ArgumentNullException.ThrowIfNull(product.ProductCategory);
        
        ArgumentNullException.ThrowIfNull(product.ProductImages);
        
        return  new ProductByIdResponseDto()
        {
            Id = product.Id,
            Name = product.Name,
            PrintDescription = product.PrintDescription,
            Description = product.Description,
            PriceInCents = product.PriceInCents,
            DiscountPercentage = product.DiscountPercentage,
            ProductCategoryId = product.ProductCategoryId,
            Category = product.ProductCategory.Category,
            ProductImages = ProductImagePresenter.ToHttp(product.ProductImages)
        };
    }
    
    public static PaginationResponseDto<ProductListResponseDto> ToHttp(IEnumerable<Product> products, int count, int page, int itemsPerPage)
    {
        var productsResponse = products.Select(product =>
        {
            ArgumentNullException.ThrowIfNull(product.ProductCategory);
            
            var principalImage = product.ProductImages.FirstOrDefault(pi => pi.DisplayOrder == 1);
            ArgumentNullException.ThrowIfNull(principalImage);

            return new ProductListResponseDto()
            {
                Id = product.Id,
                Name = product.Name,
                PriceInCents = product.PriceInCents,
                DiscountPercentage = product.DiscountPercentage,
                ProductCategoryId = product.ProductCategoryId,
                Category = product.ProductCategory.Category,
                ProductImage = ProductImagePresenter.ToHttp(principalImage),
            };
        });
        
        return PaginationHelper.FormatResponse(productsResponse, count, page, itemsPerPage);
    }
}