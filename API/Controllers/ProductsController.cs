
using API.RequestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;

using Microsoft.AspNetCore.Mvc;


namespace API.Controllers;


public class ProductsController(IGenericRepository<Product> repo) : BaseApiController
{
   
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] ProductSpecParams specParams)
    {
        var spec = new ProductSpecification(specParams);

                
        return Ok(await CreatePagedResult(repo, spec, specParams.PageIndex, specParams.PageSize));
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
