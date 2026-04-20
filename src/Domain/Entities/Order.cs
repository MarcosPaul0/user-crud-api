namespace AutoriaStore.Domain.Entities;

public class Order : Entity
{
    public int TotalPriceInCents { get; set; }
    
    public List<OrderProduct> ProductOrders { get; set; }
}