using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.SetProductImages;

public interface ISetProductImagesUseCase
{
    Task ExecuteAsync(Guid productId, SetProductImagesDto setProductImagesDto, CancellationToken cancellationToken);
}