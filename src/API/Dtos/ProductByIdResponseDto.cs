namespace UserCrud.API.Dtos;

public record ProductByIdResponseDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required int PriceInCents { get; init; }
    public required byte DiscountPercentage { get; init; }
    public required Guid ProductCategoryId { get; init; }
    public required string Category { get; init; }
    public required List<ProductImageResponseDto> ProductImages { get; init; }
}