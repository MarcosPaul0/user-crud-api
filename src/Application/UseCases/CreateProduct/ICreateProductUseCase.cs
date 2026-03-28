using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateProduct;

public interface ICreateProductUseCase
{
    Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
}