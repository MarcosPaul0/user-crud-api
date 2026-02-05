using UserCrud.Application.Interfaces;

namespace UserCrud.Infrastructure.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private const int WorkFactor = 12;
    
    public string Hash(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    public bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}