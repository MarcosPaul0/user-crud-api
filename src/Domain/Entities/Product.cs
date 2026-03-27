namespace AutoriaStore.Domain.Entities;

public class Product : Entity
{
    public string Name { get; set; }
    public string PrintDescription { get; set; }
    public string Description { get; set; }
    public int PriceInCents { get; set; }
    public int ProductionTimeInMinutes { get; set; }
    public byte DiscountPercentage { get; set; }
    public bool? IsActive { get; set; }
    public int StockQuantity { get; set; }
    
    public Guid ProductCategoryId { get; set; }
    public ProductCategory ProductCategory { get; set; }
    
    public List<ProductImage> ProductImages { get; set; }
    
    public Product(
        string name, 
        string description,
        string printDescription,
        int priceInCents, 
        int productionTimeInMinutes, 
        byte discountPercentage,
        int stockQuantity, 
        Guid productCategoryId, 
        DateTime createdAt)
    {
        Name = name;
        Description = description;
        PriceInCents = priceInCents;
        PrintDescription = printDescription;
        ProductionTimeInMinutes = productionTimeInMinutes;
        IsActive = true;
        DiscountPercentage = discountPercentage;
        StockQuantity = stockQuantity;
        ProductCategoryId = productCategoryId;
        CreatedAt = createdAt;
    }
    
    public Product(
        Guid id,
        string name,
        string printDescription,
        string description,
        int priceInCents, 
        int productionTimeInMinutes,
        bool? isActive,
        byte discountPercentage,
        int stockQuantity, 
        Guid productCategoryId, 
        ProductCategory productCategory,
        List<ProductImage> productImages,
        DateTime createdAt,
        DateTime? updatedAt)
    {
        Id = id;
        Name = name;
        PrintDescription = printDescription;
        Description = description;
        PriceInCents = priceInCents;
        ProductionTimeInMinutes = productionTimeInMinutes;
        IsActive = isActive;
        DiscountPercentage = discountPercentage;
        StockQuantity = stockQuantity;
        ProductCategoryId = productCategoryId;
        ProductCategory = productCategory;
        ProductImages = productImages;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
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