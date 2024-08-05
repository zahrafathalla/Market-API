﻿using Market.Core.Entities.Order_Aggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repository._Data.Confg.Order_Config
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(order => order.ShippingAddress,
                            shippingAddress => shippingAddress.WithOwner());

            builder.Property(order => order.Status)
                .HasConversion(
                orderStatus => orderStatus.ToString(),
                orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus)
                );

            builder.Property(order => order.Subtotal)
                    .HasColumnType("decimal(12,2)");

            builder.HasOne(order => order.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(order => order.Items)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
