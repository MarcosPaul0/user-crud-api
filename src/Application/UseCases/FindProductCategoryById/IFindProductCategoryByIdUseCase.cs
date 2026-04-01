using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductCategoryById;

public interface IFindProductCategoryByIdUseCase
{
    Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}