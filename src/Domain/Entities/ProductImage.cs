namespace UserCrud.Domain.Entities;

public class ProductImage : Entity
{
    public string ImageUrl { get; set; }
    public byte DisplayOrder { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    public ProductImage(string imageUrl, byte displayOrder, Guid productId)
    {
        ImageUrl = imageUrl;
        DisplayOrder = displayOrder;
        ProductId = productId;
    }
}