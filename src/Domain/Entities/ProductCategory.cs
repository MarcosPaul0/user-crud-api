namespace UserCrud.Domain.Entities;

public class ProductCategory : Entity
{
    public string Category { get; set; }

    public ProductCategory(string category, DateTime createdAt)
    {
        Category = category;
        CreatedAt = createdAt;
    }
}