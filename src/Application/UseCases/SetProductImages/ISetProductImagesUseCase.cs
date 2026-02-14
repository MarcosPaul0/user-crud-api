using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.SetProductImages;

public interface ISetProductImagesUseCase
{
    Task ExecuteAsync(Guid productId, SetProductImagesDto setProductImagesDto, CancellationToken cancellationToken);
}