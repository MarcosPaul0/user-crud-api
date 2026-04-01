using AutoriaStore.Domain.Enums;

namespace AutoriaStore.Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(Guid userId, UserRole role);
}