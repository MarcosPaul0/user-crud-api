namespace UserCrud.API.Dtos;

public record ProductCategoryResponseDto
{
    public required Guid Id { get; init; }
    public required string Category { get; init; }
}