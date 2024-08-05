using Market.Core.Entities.Product;
using Market.Core.Specification.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Core.ServiceContracts
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecParameters specParams);
        Task<Product?> GetProductByIdAsync(int id);
        Task<int> GetProductCountAsync(ProductSpecParameters specParams);
        Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetAllCategoriesAsync();

    }
}
