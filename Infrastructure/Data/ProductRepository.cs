using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository(StoreContext context) : IProductRepository
{
    private readonly StoreContext context = context;

    public void AddProduct(Product product)
    {
        context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        context.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetProductBrandsAsync()
    {
        return await context.Products.Select(x => x.Type)
       .Distinct()
         .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await context.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string ? sort)
    {
        var query = context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p=>p.Brand == brand);
        }
        if(!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(p=>p.Type == type);
        }
       
            query = sort switch
            {
                "priceAsc" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            }
            ;            // switch(sort)
                        // {
                        //     case "priceAsc":
                        //         query = query.OrderBy(p=>p.Price);
                        //         break;
                        //     case "priceDesc":
                        //         query = query.OrderByDescending(p=>p.Price);
                        //         break;
                        //     default:
                        //         query = query.OrderBy(p=>p.Name);
                        //         break;
                        // }
            
        return await query.ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetProductTypesAsync()
    {
        return await context.Products.Select(x=>x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return  context.Products.Any(p => p.Id == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }

   
    public void UpdateProduct(Product product)
    {
        context.Entry(product).State = EntityState.Modified;

    }
}

    
