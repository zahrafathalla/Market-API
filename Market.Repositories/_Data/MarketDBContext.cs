using Market.Core.Entities.Order_Aggregate;
using Market.Core.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository.Data
{
    public class MarketDBContext : DbContext
    {
        public MarketDBContext(DbContextOptions<MarketDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> products { get; set; }
        public DbSet<ProductBrand> brands { get; set; }
        public DbSet<ProductCategory> categories { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<DeliveryMethod> deliveryMethods { get; set; }
        public DbSet<OrderItem> orderItems { get; set; }



    }
}
