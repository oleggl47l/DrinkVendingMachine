using DrinkVendingMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DrinkVendingMachine.Infrastructure.Data.Configurations;

public class CoinConfiguration : IEntityTypeConfiguration<Coin>
{
    public void Configure(EntityTypeBuilder<Coin> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nominal).IsRequired();
        builder.Property(c => c.Quantity).HasDefaultValue(0);
        builder.Property(c => c.IsBlocked).HasDefaultValue(false);

        builder.HasData(
            new Coin { Id = 1, Nominal = 1, Quantity = 500, IsBlocked = false },
            new Coin { Id = 2, Nominal = 2, Quantity = 400, IsBlocked = false },
            new Coin { Id = 3, Nominal = 5, Quantity = 300, IsBlocked = false },
            new Coin { Id = 4, Nominal = 10, Quantity = 200, IsBlocked = false }
        );
    }
}