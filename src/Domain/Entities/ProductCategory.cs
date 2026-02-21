using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UserCrud.Domain.Entities;

public class ProductCategory : Entity
{
    public string Category { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public int ProductCount { get; set; }

    public ProductCategory(string category, DateTime createdAt)
    {
        Category = category;
        CreatedAt = createdAt;
    }
    
    public ProductCategory(Guid id, string category, int productCount, DateTime createdAt, DateTime? updatedAt)
    {
        Id = id;
        Category = category;
        ProductCount = productCount;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}