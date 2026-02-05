using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using UserCrud.Application.Interfaces;
using UserCrud.Domain.Enums;

namespace UserCrud.Infrastructure.Services;

public class JwtTokenService(IEnvironmentVariablesService environmentVariablesService) : IJwtTokenService
{
    public string GenerateToken(Guid userId, UserRole role)
    {
        var privateKey = environmentVariablesService.JwtPrivateKey;
        var issuer = environmentVariablesService.JwtIssuer;
        var audience = environmentVariablesService.JwtAudience;
        var expirationMinutes = environmentVariablesService.JwtExpirationTimeInMinutes;
        
        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);
        
        var signingCredentials = new SigningCredentials(
            new RsaSecurityKey(rsa), 
            SecurityAlgorithms.RsaSha256
        );
        
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString()),
        };
        
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}