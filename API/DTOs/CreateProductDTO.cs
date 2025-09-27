using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class CreateProductDTO
{
    [Required]
    public required string Name { get; set; } = string.Empty;
    [Required]
    public  string Description { get; set; }= string.Empty;

    [Range(0.01, double.MaxValue, ErrorMessage = "Le prix doit être supérieur à 0")]
    public decimal Price { get; set; }
    [Required]
    public string PictureUrl { get; set; }= string.Empty;
    [Required]
    public  string Type { get; set; }  = string.Empty;
    [Required]
    public  string Brand { get; set; }  = string.Empty;
    [Range(0, int.MaxValue, ErrorMessage = "La quantité en stock ne doit pas être négative")]
    public int QuantityInStock { get; set; }
}
