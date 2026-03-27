using AutoriaStore.Application.Dtos;
using UserCrud.Application.Dtos;

namespace AutoriaStore.Application.UseCases.UpdateProductCategory;

public interface IUpdateProductCategoryUseCase
{
    Task ExecuteAsync(
        Guid productCategoryId,
        UpdateProductCategoryDto createProductCategoryDto, 
        CancellationToken cancellationToken);
}