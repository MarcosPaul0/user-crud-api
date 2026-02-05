using UserCrud.Domain.Interfaces;

namespace UserCrud.Application.UseCases.ListProductCategory;

public class ListProductCategoryUseCase(IProductCategoryRepository productCategoryRepository) : IListProductCategoryUseCase
{
    public Task ExecuteAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}