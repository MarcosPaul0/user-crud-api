using Microsoft.AspNetCore.Http;

namespace AutoriaStore.Domain.Interfaces.Services;

public interface IObjectStorageService
{
    Task<string> UploadAsync(
        IFormFile file,
        string objectKey,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken);
}