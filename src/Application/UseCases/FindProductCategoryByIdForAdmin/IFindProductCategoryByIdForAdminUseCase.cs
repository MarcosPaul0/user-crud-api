using AutoriaStore.Domain.Entities;

namespace AutoriaStore.Application.UseCases.FindProductCategoryByIdForAdmin;

public interface IFindProductCategoryByIdForAdminUseCase
{
    Task<ProductCategory> ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}