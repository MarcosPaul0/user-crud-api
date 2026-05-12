using System.ComponentModel.DataAnnotations;

namespace AutoriaStore.Application.Dtos;

public record CalculateShippingDto
{
    [Required]
    public Guid ProductId { get; set; }

    [Required]
    [MinLength(8)]
    public string DestinationPostalCode { get; set; }
}