using Market.Core.Entities.Order_Aggregate;
using Market.Core.Entities.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Market.Repository.Data
{
    public static class MarketContextSeed
    {
        public async static Task SeedAsync(MarketDBContext _dbContext)
        {
            if (_dbContext.brands.Count() == 0)
            {
                var brandsData = File.ReadAllText("../Market.Repositories/_Data/DataSeed/brands.json");

                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

                ////if i had identity column and can mot seed data 
                //brands = brands.Select(b => new ProductBrand()
                //{
                //    Name = b.Name
                //}).ToList();

                if (brands?.Count() > 0)
                {
                    foreach (var brand in brands)
                    {
                        _dbContext.Set<ProductBrand>().Add(brand);
                    }
                    _dbContext.SaveChanges();
                }
            }

            if (_dbContext.categories.Count() == 0)
            {
                var categoriesData = File.ReadAllText("../Market.Repositories/_Data/DataSeed/categories.json");

                var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

                if (categories?.Count() > 0)
                {
                    //if i had identity column and can mot seed data 
                    categories = categories.Select(c => new ProductCategory()
                    {
                        Name = c.Name
                    }).ToList();

                    foreach (var category in categories)
                    {
                        _dbContext.Set<ProductCategory>().Add(category);
                    }
                    _dbContext.SaveChanges();
                }
            }

            if (_dbContext.products.Count() == 0)
            {
                var productsData = File.ReadAllText("../Market.Repositories/_Data/DataSeed/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products?.Count() > 0)
                {

                    foreach (var product in products)
                    {
                        _dbContext.Set<Product>().Add(product);
                    }
                    _dbContext.SaveChanges();
                }
            }
            if(_dbContext.deliveryMethods.Count() == 0)
            {
                var deliverMothodsData = File.ReadAllText("../Market.Repositories/_Data/DataSeed/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliverMothodsData);

                if(deliveryMethods?.Count() > 0)
                {
                    foreach(var deliveryMethod in deliveryMethods)
                    {
                        _dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
                    }
                    _dbContext.SaveChanges();

                }
            }



        }
    }
}
