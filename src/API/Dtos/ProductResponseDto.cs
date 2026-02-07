namespace UserCrud.API.Dtos;

public record ProductResponseDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int PriceInCents { get; init; }
    public required int ProductionTimeInDays { get; init; }
    public required Guid ProductCategoryId { get; init; }
    public required string Category { get; init; }
}