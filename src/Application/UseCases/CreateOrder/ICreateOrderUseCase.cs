using AutoriaStore.Application.Dtos;

namespace AutoriaStore.Application.UseCases.CreateOrder;

public interface ICreateOrderUseCase
{
    Task ExecuteAsync(
        CreateOrderDto createOrderDto,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken);
}
