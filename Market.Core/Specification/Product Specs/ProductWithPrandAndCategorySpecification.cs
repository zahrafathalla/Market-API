using Market.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification.Product_Specs
{
    public class ProductWithPrandAndCategorySpecification :BaseSpecification<Product>
    {
        // used to return all products (criteria is null) 
        public ProductWithPrandAndCategorySpecification(ProductSpecParameters specParams)
            :base ( p=>
                    (string.IsNullOrEmpty(specParams.Search)|| p.Name.ToLower().Contains(specParams.Search))&&
                    (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
                    (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value) 


                 )
        {
            //includes
            AddIncludes();

            //sorting
            if(!string.IsNullOrEmpty(specParams.sort))
            {
                switch(specParams.sort)
                {
                    case ("PriceAsc"):
                        AddOrderBy(p => p.Price);
                        break;
                    case ("PriceDec"):
                        AddOrderByDec(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                };
            }
            else
                AddOrderBy(p => p.Name);

            //pagination
            //18
            //page size =5
            //page index =2
            ApplyPagination((specParams.pageIndex-1)*specParams.PageSize ,specParams.PageSize);

        }

        //used to return product with id (set criteria where(p=> p.Id==id)
        public ProductWithPrandAndCategorySpecification(int id)
            : base(p=> p.Id==id)
        {
            AddIncludes();
        }


        private void AddIncludes()
        {
            Include.Add(p => p.Brand);
            Include.Add(p => p.Category);
        }
    }
}
