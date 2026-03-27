using System.Text.Json.Serialization;

namespace AutoriaStore.Domain.Entities;

public class Phone : Entity
{
    public string? PhoneNumber { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    public Phone(string phoneNumber)
    {
        PhoneNumber = phoneNumber;
    }
}