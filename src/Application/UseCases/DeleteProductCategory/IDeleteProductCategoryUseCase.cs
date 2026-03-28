namespace AutoriaStore.Application.UseCases.DeleteProductCategory;

public interface IDeleteProductCategoryUseCase
{
    Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}