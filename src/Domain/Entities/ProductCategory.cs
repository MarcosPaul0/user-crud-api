using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AutoriaStore.Domain.Entities;

public class ProductCategory : Entity
{
    public string Category { get; set; }
    
    public bool IsActive { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public int ProductCount { get; set; }
}