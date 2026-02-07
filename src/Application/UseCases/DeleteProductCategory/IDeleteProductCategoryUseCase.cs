namespace UserCrud.Application.UseCases.DeleteProductCategory;

public interface IDeleteProductCategoryUseCase
{
    Task ExecuteAsync(Guid productCategoryId, CancellationToken cancellationToken);
}