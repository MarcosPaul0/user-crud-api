using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.CreateProduct;

public interface ICreateProductUseCase
{
    Task ExecuteAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
}