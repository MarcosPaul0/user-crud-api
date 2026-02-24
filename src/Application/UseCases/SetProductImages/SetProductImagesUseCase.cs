using Microsoft.AspNetCore.Http;
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

        var productImagesToCreate = new List<(IFormFile, ProductImage)>();
        var productImagesToUpdate = new List<(IFormFile?, ProductImage)>();

        var index = 0;
        foreach (var productImageDto in setProductImagesDto.Images)
        {
            if (productImageDto.Id != null && productImageDto.Id != Guid.Empty)
            {
                continue;
            }
            
            var productImage = new ProductImage(null, productImageDto.DisplayOrder, productId, DateTime.UtcNow);
            
            productImagesToCreate.Add((productImageDto.File!, productImage));
            index++;
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
            
            productImage.DisplayOrder = productImageDto.DisplayOrder;
            productImage.UpdatedAt = DateTime.UtcNow;
            
            productImagesToUpdate.Add((productImageDto.File, productImage));
        }

        foreach (var (file, productImage) in productImagesToCreate)
        {
            await CreateProductImagesAsync(productId, file, productImage, cancellationToken);
        }
        
        foreach (var (file, productImage) in productImagesToUpdate)
        {
            await UpdateProductImagesAsync(productImage, file, cancellationToken);
        }
        
        await unitOfWork.SaveChangesAsync();
    }

    private async Task CreateProductImagesAsync(
        Guid productId,
        IFormFile file,
        ProductImage productImage, 
        CancellationToken cancellationToken)
    {
        if (file == null)
        {
            throw new ConflictException(ExceptionMessages.PRODUCT_IMAGE_FILE_IS_REQUIRED);
        }
        
        var imageId = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        var objectKey = $"products/{productId}/{imageId}{extension}";
        
        var imageUrl = await objectStorageService.UploadAsync(
            file,
            objectKey,
            cancellationToken);
        
        productImage.ImageUrl = imageUrl;
            
        await productImageRepository.CreateAsync(productImage, cancellationToken);
    }

    private async Task UpdateProductImagesAsync(
        ProductImage productImage,
        IFormFile? file,
        CancellationToken cancellationToken)
    {
        if (file != null)
        {
            var oldKey = ObjectStorageHelper.ExtractObjectKey(productImage.ImageUrl);
            await objectStorageService.DeleteAsync(oldKey, cancellationToken);

            var extension = Path.GetExtension(file.FileName);
            var objectKey = $"products/{productImage.ProductId}/{productImage.Id}{extension}";

            var newUrl = await objectStorageService.UploadAsync(
                file,
                objectKey,
                cancellationToken);

            productImage.ImageUrl = newUrl;
        }
        
        await productImageRepository.UpdateAsync(productImage, cancellationToken);
    }
}