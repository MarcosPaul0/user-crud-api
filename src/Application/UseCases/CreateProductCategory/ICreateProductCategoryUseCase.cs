using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateProductCategory;

public interface ICreateProductCategoryUseCase
{
    Task ExecuteAsync(CreateProductCategoryDto createProductCategoryDto, CancellationToken cancellationToken);
}