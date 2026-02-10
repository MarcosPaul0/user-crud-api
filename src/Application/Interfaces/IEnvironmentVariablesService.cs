namespace UserCrud.Application.Interfaces;

public interface IEnvironmentVariablesService
{
    public string JwtPrivateKey { get; }
    public string JwtPublicKey { get; }
    public string JwtIssuer { get; }
    public string JwtAudience { get; }
    public int JwtExpirationTimeInMinutes { get; }
    public string ObjectStorageBucket { get; }
    public string ObjectStoragePublicUrl { get; }
    public string ObjectStorageEndpoint { get; }
    public string ObjectStorageAccessKey { get; }
    public string ObjectStorageSecretKey { get; }
}