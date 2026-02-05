using UserCrud.Application.Dtos;

namespace UserCrud.Application.UseCases.CreateProductCategory;

public interface ICreateProductCategoryUseCase
{
    Task ExecuteAsync(CreateProductCategoryDto createProductCategoryDto, CancellationToken cancellationToken);
}