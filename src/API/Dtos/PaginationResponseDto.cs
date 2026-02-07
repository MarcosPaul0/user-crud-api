namespace UserCrud.API.Dtos;

public record PaginationResponseDto<T>
{
    public required IEnumerable<T> Items { get; init; }
    public required int TotalPages { get; init; }
    public required int TotalItems { get; init; }
    public required int ItemsPerPage { get; init; }
    public required int Page { get; init; }
    public required bool HasPrevious { get; init; }
    public required bool HasNext { get; init; }
}