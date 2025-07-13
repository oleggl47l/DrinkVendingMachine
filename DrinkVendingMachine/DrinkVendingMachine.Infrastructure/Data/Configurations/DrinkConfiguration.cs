using DrinkVendingMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkVendingMachine.Infrastructure.Data.Configurations;

public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
{
    public void Configure(EntityTypeBuilder<Drink> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Property(d => d.Quantity).HasDefaultValue(0);

        builder.HasOne(d => d.Brand)
            .WithMany(b => b.Drinks)
            .HasForeignKey(d => d.BrandId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}