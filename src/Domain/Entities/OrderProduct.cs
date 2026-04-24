namespace AutoriaStore.Domain.Entities;

public class OrderProduct : Entity
{
    public int Quantity { get; set; }
    public int UnitPriceInCents { get; set; }
    
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
}