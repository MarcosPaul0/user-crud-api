namespace UserCrud.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int PriceInCents { get; set; }
    public int ProductionTimeInDays { get; set; }
    public bool? IsActive { get; set; }
    public int StockQuantity { get; set; }
    
    public Guid ProductCategoryId { get; set; }
    public ProductCategory ProductCategory { get; set; }
    
    public Product(
        string name, 
        string description,
        int priceInCents, 
        int productionTimeInDays, 
        int stockQuantity, 
        Guid productCategoryId, 
        DateTime createdAt)
    {
        Name = name;
        Description = description;
        PriceInCents = priceInCents;
        ProductionTimeInDays = productionTimeInDays;
        IsActive = true;
        StockQuantity = stockQuantity;
        ProductCategoryId = productCategoryId;
        CreatedAt = createdAt;
    }

    public Product(string? name, Guid? productCategoryId)
    {
        if (name != null)
        {
            Name = name;
        }
        
        if (productCategoryId != null)
        {
            ProductCategoryId = productCategoryId.Value;
        }
    }
    
    public Product(string? name, Guid? productCategoryId, bool? isActive)
    {
        if (name != null)
        {
            Name = name;
        }
        
        if (productCategoryId != null)
        {
            ProductCategoryId = productCategoryId.Value;
        }
        
        if (isActive != null)
        {
            IsActive = isActive;
        }
    }
}