namespace AutoriaStore.Domain.Dto.Services;

public record CreateIdempotencyKeyDto
{
    public required Guid AuthenticatedUserId { get; init; }
    public required string IdempotencyKey { get; init; }
    public required string Endpoint { get; init; }
    public required int StatusCode { get; init; }
    public required object RequestObject { get; init; }
    public required object? ResponseObject { get; init; }
}