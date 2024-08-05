using Market.Core;
using Market.Core.Entities.Product;
using Market.Core.ServiceContracts;
using Market.Core.Specification.Product_Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Product>> GetAllProductsAsync(ProductSpecParameters specParams)
        {
            var spec = new ProductWithPrandAndCategorySpecification(specParams);

            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

            return products;       
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithPrandAndCategorySpecification(id);

            var product = await _unitOfWork.Repository<Product>().GetByIdWithSpecAsync(spec);

            return product;

        }
        public async Task<int> GetProductCountAsync(ProductSpecParameters specParams)
        {
            var count = await _unitOfWork.Repository<Product>().GetCountAsync(new ProductWithFilterationForCountSpecification(specParams));

            return count;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();

            return brands;
        }

        public async Task<IReadOnlyList<ProductCategory>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();

            return categories;
        }

       
    }
}
