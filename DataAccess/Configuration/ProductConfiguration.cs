using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("product");
            builder.HasKey(x => x.ProductId);
            builder.Property(x => x.UnitPrice);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Weight);
            builder.Property(x => x.UnitInStock);
            builder.HasOne(x => x.Category).WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
