namespace UserCrud.Application.UseCases.ListProductCategory;

public interface IListProductCategoryUseCase
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}