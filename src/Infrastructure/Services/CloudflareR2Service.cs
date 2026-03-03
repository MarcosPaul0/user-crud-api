using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserCrud.Application.Interfaces;

namespace UserCrud.Infrastructure.Services;

public class CloudflareR2Service(
    ILogger<CloudflareR2Service> logger,
    IAmazonS3 s3, 
    IEnvironmentVariablesService environmentVariablesService) : IObjectStorageService
{
    public async Task<string> UploadAsync(
        IFormFile file,
        string objectKey,
        CancellationToken cancellationToken)
    {
        try
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
            
            logger.LogError("File uploaded from Cloudflare R2 successfully.");

            return $"{environmentVariablesService.ObjectStoragePublicUrl}/{objectKey}";
        } catch (Exception exception)
        {
            logger.LogError(exception, "Error uploading file to Cloudflare R2.");
            throw;
        }
    }

    public async Task DeleteAsync(
        string objectKey,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = environmentVariablesService.ObjectStorageBucket,
                Key = objectKey
            };

            await s3.DeleteObjectAsync(request, cancellationToken);
            
            logger.LogError("File deleted from Cloudflare R2 successfully.");
        } catch (Exception exception)
        {
            logger.LogError(exception, "Error deleting file to Cloudflare R2.");
            throw;
        }
    }
}