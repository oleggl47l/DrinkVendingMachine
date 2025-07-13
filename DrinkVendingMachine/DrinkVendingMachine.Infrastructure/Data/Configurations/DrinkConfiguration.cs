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
        
        builder.HasData(
            new Drink { Id = 1, Name = "Coca-Cola Classic 0.33L", Price = 82, Quantity = 15, BrandId = 1, ImageUrl = "https://avatars.mds.yandex.net/i?id=e92740110da039f1caea551d11cf271a14e5291b-4261589-images-thumbs&n=13" },
            new Drink { Id = 2, Name = "Pepsi 0.33L", Price = 80, Quantity = 12, BrandId = 2, ImageUrl = "https://avatars.mds.yandex.net/i?id=0a2126d6b7a42414a80f9fe2a8d5d0fc6ae9bc20-12569871-images-thumbs&n=13" },
            new Drink { Id = 3, Name = "Fanta Orange 0.33L", Price = 84, Quantity = 10, BrandId = 3, ImageUrl = "https://avatars.mds.yandex.net/i?id=ed98493dc133b26db2aae9a1dd732ee0eda8a7e9-5263755-images-thumbs&n=13" },
            new Drink { Id = 4, Name = "Sprite Lemon-Lime 0.33L", Price = 75, Quantity = 8, BrandId = 4, ImageUrl = "https://avatars.mds.yandex.net/i?id=c1bc9eca3c1bba813ec634c0b00b5a957313f00c-4462454-images-thumbs&n=13" },
            new Drink { Id = 5, Name = "Mountain Dew 0.33L", Price = 85, Quantity = 5, BrandId = 5, ImageUrl = "https://avatars.mds.yandex.net/i?id=37f21a1246146e0582ee6f20b26efae9ef8c57dd-5664541-images-thumbs&n=13" },
            new Drink { Id = 6, Name = "7Up 0.33L", Price = 72, Quantity = 7, BrandId = 6, ImageUrl = "https://avatars.mds.yandex.net/i?id=dea8080e40047cb9a537383f0e33e92d14935301-8183104-images-thumbs&n=13" },
            new Drink { Id = 7, Name = "Coca-Cola Zero 0.33L", Price = 83, Quantity = 9, BrandId = 1, ImageUrl = "https://avatars.mds.yandex.net/i?id=2eeb95cb38c3725a08e95d446e52239ab1ccbc58-12928152-images-thumbs&n=13" },
            new Drink { Id = 8, Name = "Pepsi Max 0.33L", Price = 100, Quantity = 6, BrandId = 2, ImageUrl = "https://avatars.mds.yandex.net/i?id=bbd1651c52ac2f21a3b6375f0bf31e7562c3fd63-4219714-images-thumbs&n=13" }
        );
    }
}