using UserCrud.Domain.Enums;

namespace UserCrud.Domain.Entities;

public class User : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public UserRole Role { get; set; }

    public User(string name, string email, string password, UserRole role, DateTime createdAt) 
    {
        Name = name;
        Email = email;
        Password = password;
        Role = role;
        CreatedAt = createdAt;
    }
}