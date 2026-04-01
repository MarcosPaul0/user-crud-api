using Amazon.S3;
using Amazon.S3.Model;
using AutoriaStore.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AutoriaStore.Application.Interfaces;

namespace AutoriaStore.Infrastructure.Services;

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
            
            logger.LogInformation("File uploaded from Cloudflare R2 successfully.");

            return $"{environmentVariablesService.ObjectStoragePublicUrl}/{objectKey}";
        } catch (Exception exception)
        {
            logger.LogError(exception, "Error uploading file to Cloudflare R2.");
            throw;
        }
    }

    public async Task DeleteAsync(
        string imageUrl,
        CancellationToken cancellationToken)
    {
        try
        {
            var objectKey = ObjectStorageHelper.ExtractObjectKey(imageUrl);
            
            var request = new DeleteObjectRequest
            {
                BucketName = environmentVariablesService.ObjectStorageBucket,
                Key = objectKey
            };

            await s3.DeleteObjectAsync(request, cancellationToken);
            
            logger.LogInformation("File deleted from Cloudflare R2 successfully.");
        } catch (Exception exception)
        {
            logger.LogError(exception, "Error deleting file to Cloudflare R2.");
            throw;
        }
    }
}