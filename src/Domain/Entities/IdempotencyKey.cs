namespace AutoriaStore.Domain.Entities;

public class IdempotencyKey : Entity
{
    public Guid UserId { get; set; }
    public string Key { get; set; }
    public string Endpoint { get; set; }
    public string RequestHash { get; set; }
    public int ResponseStatus { get; set; }
    public string? ResponseBody { get; set; }
    public DateTime ExpiresAt { get; set; }
}
