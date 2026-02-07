using UserCrud.API.Dtos;

namespace UserCrud.API.Helpers;

public static class PaginationHelper
{
    public static PaginationResponseDto<T> FormatResponse<T>(IEnumerable<T> items, int totalItems, int page, int itemsPerPage)
    {
        var totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
        
        var hasNextPage = page + 1 <= totalPages;;
        
        var hasPreviousPage = page - 1 >= 1;
        
        return new PaginationResponseDto<T>()
        {
            Items = items,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Page = page,
            HasNext = hasNextPage,
            HasPrevious = hasPreviousPage,
            ItemsPerPage = itemsPerPage,
        };
    }
}