using DataAccess.Configuration;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataContext
{
    public class EStoreDbContext : IdentityDbContext<User>
    {
        private readonly DbContextOptions _options;
        public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderDetailsConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        #region entity
        public DbSet<Product> Products { get; set;}
        public DbSet<Order> Orders { get; set;}
        public DbSet<OrderDetail> OrderDetails { get; set;}
        public DbSet<Category> Category { get; set;}
        #endregion
    }
}
