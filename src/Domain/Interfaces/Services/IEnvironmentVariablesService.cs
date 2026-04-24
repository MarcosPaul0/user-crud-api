namespace AutoriaStore.Domain.Interfaces.Services;

public interface IEnvironmentVariablesService
{
    public string Origin { get; }
    
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
    
    public string AuthTokenCookie { get; }
    
    public string OriginPostalCode { get; }
    public string PostageApiKey { get; }
    public string PostageApiServiceCode  { get; }
}