using DrinkVendingMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkVendingMachine.Infrastructure.Data.Configurations;

public class BrandConfiguration : IEntityTypeConfiguration<Brand>
{
    public void Configure(EntityTypeBuilder<Brand> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Name).IsRequired().HasMaxLength(50);
        
        builder.HasData(
            new Brand { Id = 1, Name = "Coca-Cola" },
            new Brand { Id = 2, Name = "Pepsi" },
            new Brand { Id = 3, Name = "Fanta" },
            new Brand { Id = 4, Name = "Sprite" },
            new Brand { Id = 5, Name = "Mountain Dew" },
            new Brand { Id = 6, Name = "7Up" }
        );
    }
}