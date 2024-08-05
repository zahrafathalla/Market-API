using Market.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.Specification.Product_Specs
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParameters specParams)
            : base(p =>
                    (!specParams.brandId.HasValue || p.BrandId == specParams.brandId.Value) &&
                    (!specParams.categoryId.HasValue || p.CategoryId == specParams.categoryId.Value)

                 )

        {
        }
    }
}
