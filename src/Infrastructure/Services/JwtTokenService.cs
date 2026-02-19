using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Enums;

namespace UserCrud.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;
    private readonly SigningCredentials _signingCredentials;

    public JwtTokenService(IEnvironmentVariablesService environmentVariablesService)
    {
        _issuer = environmentVariablesService.JwtIssuer;
        _audience = environmentVariablesService.JwtAudience;
        _expirationMinutes = environmentVariablesService.JwtExpirationTimeInMinutes;

        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(environmentVariablesService.JwtPrivateKey), out _);
        _signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(Guid userId, UserRole role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(_expirationMinutes),
            signingCredentials: _signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}