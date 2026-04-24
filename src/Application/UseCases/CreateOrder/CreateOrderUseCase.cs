using AutoriaStore.Application.Dtos;
using AutoriaStore.Application.Exceptions;
using AutoriaStore.Domain.Dto.Services;
using AutoriaStore.Domain.Entities;
using AutoriaStore.Domain.Interfaces;
using AutoriaStore.Domain.Interfaces.Repositories;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Application.UseCases.CreateOrder;

public sealed class CreateOrderUseCase(
    IAuthenticatedUserService authenticatedUserService,
    IIdempotencyService idempotencyService,
    IUnitOfWork unitOfWork) : ICreateOrderUseCase
{
    public async Task ExecuteAsync(
        CreateOrderDto createOrderDto,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken)
    {
        var authenticatedUserId = authenticatedUserService.GetUserId();

        if (authenticatedUserId == null || authenticatedUserId == Guid.Empty)
        {
            throw new UnauthorizeException(ExceptionMessages.USER_NOT_AUTHENTICATED);
        }

        if (string.IsNullOrWhiteSpace(idempotencyKey))
        {
            throw new BadRequestException(ExceptionMessages.IDEMPOTENCY_KEY_REQUIRED);
        }

        if (createOrderDto.Items.Count == 0)
        {
            throw new ConflictException(ExceptionMessages.ORDER_ITEMS_REQUIRED);
        }
        
        var existingIdempotencyKey = await idempotencyService.GetIdempotencyKeyAsync(
            authenticatedUserId.Value, 
            idempotencyKey, 
            endpoint,
            cancellationToken);

        if (existingIdempotencyKey is null)
        {
            return;
        }
        
        var idempotencyKeyIsExpired = await idempotencyService.RemoveIdempotencyKeyIfExpiredAsync(
            existingIdempotencyKey, 
            cancellationToken);

        if (!idempotencyKeyIsExpired)
        {
            return;
        }

        await CreateOrderAsync(createOrderDto, authenticatedUserId.Value, idempotencyKey, endpoint, cancellationToken);
    }

    private async Task CreateOrderAsync(
        CreateOrderDto createOrderDto, 
        Guid authenticatedUserId,
        string idempotencyKey,
        string endpoint,
        CancellationToken cancellationToken)
    {
        var orderProducts = new List<OrderProduct>(createOrderDto.Items.Count);
        var totalPriceInCents = 0;

        foreach (var item in createOrderDto.Items)
        {
            if (item.Quantity <= 0)
            {
                throw new ConflictException(ExceptionMessages.ORDER_ITEM_QUANTITY_INVALID);
            }

            var product = await unitOfWork.Product.FindByIdAsync(item.ProductId, cancellationToken);

            if (product is not { IsActive: true })
            {
                throw new NotFoundException(ExceptionMessages.PRODUCT_NOT_FOUND);
            }

            totalPriceInCents += product.PriceInCents * item.Quantity;

            orderProducts.Add(new OrderProduct
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPriceInCents = product.PriceInCents,
                CreatedAt = DateTime.UtcNow
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = authenticatedUserId,
            TotalPriceInCents = totalPriceInCents,
            ProductOrders = orderProducts,
            CreatedAt = DateTime.UtcNow
        };

        var newIdempotencyKey = new CreateIdempotencyKeyDto()
        {
            ResponseObject = null,
            RequestObject = createOrderDto,
            StatusCode = StatusCodes.Status204NoContent,
            AuthenticatedUserId = authenticatedUserId,
            Endpoint = endpoint,
            IdempotencyKey = idempotencyKey,
        };

        await idempotencyService.CreateIdempotencyKeyAsync(newIdempotencyKey, cancellationToken);
        
        await unitOfWork.Order.CreateAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync();
    }
}
