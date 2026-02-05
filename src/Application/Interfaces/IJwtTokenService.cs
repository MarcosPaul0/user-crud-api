using UserCrud.Domain.Enums;

namespace UserCrud.Application.Interfaces;

public interface IJwtTokenService
{
    public string GenerateToken(Guid userId, UserRole role);
}