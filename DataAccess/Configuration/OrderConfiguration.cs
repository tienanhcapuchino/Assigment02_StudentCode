using BussinessObject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configuration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("order");
            builder.HasKey(x => x.OrderId);
            builder.Property(x => x.MemberId);
            builder.Property(x => x.RequiredDate);
            builder.Property(x => x.OrderDate);
            builder.Property(x => x.ShipDate);
            builder.Property(x => x.Freight);
            builder.HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.MemberId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
