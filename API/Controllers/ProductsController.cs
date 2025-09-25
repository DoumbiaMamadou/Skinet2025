using System;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/controller")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string ? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);

        var products = await repo.ListAsync(spec);
        
        if (products == null || !products.Any())
        {
            return NotFound("Aucun produit trouvé.");
        }
        
        return Ok(products);
    }

    [HttpGet("{id:int}")] //api/products/3
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        repo.Add(product);

        if(await repo.SaveAllAsync())
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

        repo.Update(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Impossible de mettre à jour le produit.");
        
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }
        repo.Remove(product);

        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }
        return BadRequest("Impossible de supprimer le produit. ");
    }

    [HttpGet("brands")]
     public async Task<ActionResult<IReadOnlyList<string>>> GetProductBrands()
     {
        var spec = new BrandListSpecification();
        var brands = await repo.ListAsync(spec);
         return Ok(await repo.ListAsync(spec)  );
     }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypesAsync()
    {
        var spec = new TypeListSpecification();
        var types = await repo.ListAsync(spec);
        return Ok(await repo.ListAsync(spec));
         
     }
    private bool ProductExists(int id)
    {
        return repo.Exists(id);
    }
}
