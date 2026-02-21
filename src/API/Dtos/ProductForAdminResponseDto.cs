using UserCrud.Domain.Entities;

namespace UserCrud.API.Dtos;

public record ProductForAdminResponseDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int PriceInCents { get; init; }
    public required int ProductionTimeInMinutes { get; init; }
    public required byte DiscountPercentage { get; init; }
    public required bool? IsActive { get; init; }
    public required int StockQuantity { get; init; }
    public required Guid ProductCategoryId { get; init; }
    public required string Category { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
}