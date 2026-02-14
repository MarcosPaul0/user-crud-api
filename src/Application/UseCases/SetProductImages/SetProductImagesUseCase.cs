using UserCrud.Application.Dtos;
using UserCrud.Application.Exceptions;
using UserCrud.Application.Helpers;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Entities;
using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.SetProductImages;

public class SetProductImagesUseCase(
    IObjectStorageService objectStorageService,
    IProductRepository productRepository,
    IProductImageRepository productImageRepository, 
    IUnitOfWork unitOfWork) : ISetProductImagesUseCase
{
    public async Task ExecuteAsync(
        Guid productId, 
        SetProductImagesDto setProductImagesDto, 
        CancellationToken cancellationToken)
    {
        var product = await productRepository.FindByIdAsync(productId, cancellationToken);

        if (product == null)
        {
            throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
        }

        var productImagesToCreate = new List<ProductImageDto>();
        var productImagesToUpdate = new List<(ProductImageDto, ProductImage)>();

        foreach (var productImageDto in setProductImagesDto.Images)
        {
            if (productImageDto.Id != null && productImageDto.Id != Guid.Empty)
            {
                continue;
            }
            
            productImagesToCreate.Add(productImageDto);
        }

        var productImages = await productImageRepository.FindAllByProductIdAsync(productId, cancellationToken);

        if (productImages.Count + productImagesToCreate.Count > 5)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_MAX_IMAGES_REACHED);
        }
        
        foreach (var productImageDto in setProductImagesDto.Images)
        {
            if (productImageDto.Id == null || productImageDto.Id == Guid.Empty)
            {
                continue;
            }
            
            var productImage = await productImageRepository.FindByIdAsync(productImageDto.Id.Value, cancellationToken);

            if (productImage == null)
            {
                throw new NotFoundException(ExceptionMessages.PRODUCT_IMAGE_NOT_FOUND);
            }
            
            productImagesToUpdate.Add((productImageDto, productImage));
        }

        foreach (var image in productImagesToCreate)
        {
            await CreateProductImagesAsync(productId, image, cancellationToken);
        }
        
        foreach (var (dto, entity) in productImagesToUpdate)
        {
            await UpdateProductImagesAsync(entity, dto, cancellationToken);
        }
        
        await unitOfWork.SaveChangesAsync();
    }

    private async Task CreateProductImagesAsync(
        Guid productId, 
        ProductImageDto productImageDto, 
        CancellationToken cancellationToken)
    {
        if (productImageDto.File == null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_IMAGE_FILE_IS_REQUIRED);
        }
        
        var imageId = Guid.NewGuid();
        var extension = Path.GetExtension(productImageDto.File.FileName);
        var objectKey = $"products/{productId}/{imageId}{extension}";
        
        var imageUrl = await objectStorageService.UploadAsync(
            productImageDto.File,
            objectKey,
            cancellationToken);
        
        var productImage = new ProductImage(imageUrl, productImageDto.DisplayOrder, productId);
            
        await productImageRepository.CreateAsync(productImage, cancellationToken);
    }

    private async Task UpdateProductImagesAsync(
        ProductImage productImage, 
        ProductImageDto productImageDto, 
        CancellationToken cancellationToken)
    {
        if (productImageDto.File != null)
        {
            var oldKey = ObjectStorageHelper.ExtractObjectKey(productImage.ImageUrl);
            await objectStorageService.DeleteAsync(oldKey, cancellationToken);

            var extension = Path.GetExtension(productImageDto.File.FileName);
            var objectKey = $"products/{productImage.ProductId}/{productImage.Id}{extension}";

            var newUrl = await objectStorageService.UploadAsync(
                productImageDto.File,
                objectKey,
                cancellationToken);

            productImage.ImageUrl = newUrl;
        }
        
        productImage.DisplayOrder = productImageDto.DisplayOrder;
        productImage.UpdatedAt = DateTime.UtcNow;
        
        await productImageRepository.UpdateAsync(productImage, cancellationToken);
    }
}