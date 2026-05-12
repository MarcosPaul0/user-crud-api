// <copyright file="JwtTokenService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using AutoriaStore.Domain.Enums;
using AutoriaStore.Domain.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace AutoriaStore.Infrastructure.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly string issuer;
    private readonly string audience;
    private readonly int expirationMinutes;
    private readonly SigningCredentials signingCredentials;

    public JwtTokenService(IEnvironmentVariablesService environmentVariablesService)
    {
        this.issuer = environmentVariablesService.JwtIssuer;
        this.audience = environmentVariablesService.JwtAudience;
        this.expirationMinutes = environmentVariablesService.JwtExpirationTimeInMinutes;

        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(environmentVariablesService.JwtPrivateKey), out _);
        this.signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(Guid userId, UserRole role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: this.issuer,
            audience: this.audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(this.expirationMinutes),
            signingCredentials: this.signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}