using DrinkVendingMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkVendingMachine.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(i => i.DrinkName).IsRequired().HasMaxLength(100);
        builder.Property(i => i.BrandName).IsRequired().HasMaxLength(50);
        builder.Property(i => i.Quantity).IsRequired();
    }
}