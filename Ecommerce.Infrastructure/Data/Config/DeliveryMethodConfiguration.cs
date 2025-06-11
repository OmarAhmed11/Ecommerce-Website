using Ecommerce.Core.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Config
{
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DeliveryMethod>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethod> builder)
        {
            builder.Property(o => o.Price).HasColumnType("decimal(18,2)");
            builder.HasData(
                new DeliveryMethod { Id = 1,Name = "FedEx", DeliveryTime = "1 week", Description = "Fast delivery all over the world", Price = 100 },
                new DeliveryMethod { Id = 2,Name = "Bosta", DeliveryTime = "2 week", Description = "Fast delivery For Locals", Price = 50 }
            );
        }
    }
}
