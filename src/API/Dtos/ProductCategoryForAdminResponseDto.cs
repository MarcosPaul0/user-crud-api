namespace AutoriaStore.API.Dtos;

public record ProductCategoryForAdminResponseDto
{
    public required Guid Id { get; init; }
    public required string Category { get; init; }
    public required bool IsActive { get; init; }
    public required int ProductCount { get; init; }
    public required DateTime CreatedAt { get; init; }
}