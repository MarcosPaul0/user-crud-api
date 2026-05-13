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
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationMinutes;
    private readonly SigningCredentials _signingCredentials;

    public JwtTokenService(IEnvironmentVariablesService environmentVariablesService)
    {
        this._issuer = environmentVariablesService.JwtIssuer;
        this._audience = environmentVariablesService.JwtAudience;
        this._expirationMinutes = environmentVariablesService.JwtExpirationTimeInMinutes;

        var rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(Convert.FromBase64String(environmentVariablesService.JwtPrivateKey), out _);
        this._signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);
    }

    public string GenerateToken(Guid userId, UserRole role)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: this._issuer,
            audience: this._audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(this._expirationMinutes),
            signingCredentials: this._signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}