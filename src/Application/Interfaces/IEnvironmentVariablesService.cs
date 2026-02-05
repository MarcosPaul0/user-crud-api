namespace UserCrud.Application.Interfaces;

public interface IEnvironmentVariablesService
{
    public string JwtPrivateKey { get; }
    public string JwtPublicKey { get; }
    public string JwtIssuer { get; }
    public string JwtAudience { get; }
    public int JwtExpirationTimeInMinutes { get; }
}