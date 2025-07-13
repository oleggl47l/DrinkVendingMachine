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
    }
}