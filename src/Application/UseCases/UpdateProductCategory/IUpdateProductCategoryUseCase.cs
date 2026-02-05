using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.UpdateProductCategory;

public interface IUpdateProductCategoryUseCase
{
    Task ExecuteAsync(
        Guid productCategoryId,
        UpdateProductCategoryDto createProductCategoryDto, 
        CancellationToken cancellationToken);
}