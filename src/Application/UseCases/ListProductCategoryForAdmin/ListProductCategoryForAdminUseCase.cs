using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;

namespace AutoriaStore.Application.UseCases.ListProductCategoryForAdmin;

public class ListProductCategoryForAdminUseCase(IUnitOfWork unitOfWork) : IListProductCategoryForAdminUseCase
{
    public async Task<(List<ProductCategory> productCategories, int count)> ExecuteAsync(CancellationToken cancellationToken)
    {
        var productCategories = await unitOfWork.ProductCategory.FindAllAsync(null, cancellationToken);

        return (productCategories, productCategories.Count());
    }
}