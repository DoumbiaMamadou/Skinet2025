using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification: BaseSpecification<Product>//herite de BaseSpecification, ses propriétes et méthodes
{
    public ProductSpecification(ProductSpecParams specParams): base(x =>
    (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
    (!specParams.Brands.Any() || specParams.Brands.Contains(x.Brand)) &&
     (!specParams.Types.Any() || specParams.Types.Contains(x.Type))
    )
    {
        ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize); //


        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderBy(p => p.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(p => p.Price);
                break;
            default:
                AddOrderBy(p => p.Name);
                break;
        }
    }
}
