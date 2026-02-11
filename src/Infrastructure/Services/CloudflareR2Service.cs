using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using UserCrud.Application.Interfaces;

namespace UserCrud.Infrastructure.Services;

public class CloudflareR2Service(
    IAmazonS3 s3, 
    IEnvironmentVariablesService environmentVariablesService) : IObjectStorageService
{
    public async Task<string> UploadAsync(
        IFormFile file,
        string objectKey,
        CancellationToken cancellationToken)
    {
        await using var stream = file.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = environmentVariablesService.ObjectStorageBucket,
            Key = objectKey,
            InputStream = stream,
            ContentType = file.ContentType,
            DisablePayloadSigning = true
        };

        await s3.PutObjectAsync(request, cancellationToken);

        return $"{environmentVariablesService.ObjectStoragePublicUrl}/{objectKey}";
    }

    public async Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = environmentVariablesService.ObjectStorageBucket,
            Key = objectKey
        };

        await s3.DeleteObjectAsync(request, cancellationToken);
    }
}