using System;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/controller")]
public class ProductsController(IProductRepository repo) : ControllerBase
{
   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string ? sort)
    {
        
        return Ok(await repo.GetProductsAsync(brand, type , sort));
    }

    [HttpGet("{id:int}")] //api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.AddProduct(product);

        if(await repo.SaveChangesAsync())
        { return CreatedAtAction("GetProduct", new { id = product.Id }, product);  }
            
        
        return BadRequest("Impossible d'ajouter le produit.");        
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
        {
            return BadRequest("Ce produit ne peut être modifié.");
        }

        repo.UpdateProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }

        return BadRequest("Impossible de mettre à jour le produit.");
        
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetProductByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        repo.DeleteProduct(product);

        if (await repo.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Impossible de supprimer le produit. ");
    }

    [HttpGet("brands")]
     public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
     {
         return Ok( await repo.GetProductBrandsAsync());
     }

     [HttpGet("types")]
     public async Task<ActionResult<IReadOnlyList<string>>> GetTypesAsync()
     {
         return Ok( await repo.GetProductTypesAsync());
     }
    private bool ProductExists(int id)
    {
        return repo.ProductExists(id);
    }
}
