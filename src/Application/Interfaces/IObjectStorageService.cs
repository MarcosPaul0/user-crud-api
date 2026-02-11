using Microsoft.AspNetCore.Http;

namespace UserCrud.Application.Interfaces;

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