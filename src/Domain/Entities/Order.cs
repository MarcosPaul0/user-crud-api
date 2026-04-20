namespace AutoriaStore.Domain.Entities;

public class Order : Entity
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public int TotalPriceInCents { get; set; }
    
    public List<OrderProduct> ProductOrders { get; set; }
}
