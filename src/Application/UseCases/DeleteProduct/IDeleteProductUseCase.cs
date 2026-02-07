namespace UserCrud.Application.UseCases.DeleteProduct;

public interface IDeleteProductUseCase
{
    Task ExecuteAsync(Guid productId, CancellationToken cancellationToken);
}