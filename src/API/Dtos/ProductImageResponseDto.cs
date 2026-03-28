namespace AutoriaStore.API.Dtos;

public record ProductImageResponseDto
{
    public required Guid Id { get; init; }
    public required string ImageUrl { get; init; }
    public required byte DisplayOrder { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required DateTime? UpdatedAt { get; init; }
}