using AutoriaStore.Domain.Entities;
using UserCrud.API.Dtos;

namespace UserCrud.API.Presenters;

public static class ProductImagePresenter
{
    public static ProductImageResponseDto ToHttp(ProductImage productImage)
    {
        return new ProductImageResponseDto()
        {
            Id = productImage.Id,
            ImageUrl = productImage.ImageUrl,
            DisplayOrder = productImage.DisplayOrder,
            CreatedAt = productImage.CreatedAt,
            UpdatedAt = productImage.UpdatedAt,
        };
    }
    
    public static List<ProductImageResponseDto> ToHttp(List<ProductImage> productImages)
    {
        return productImages.Select(ToHttp).ToList();
    }
}