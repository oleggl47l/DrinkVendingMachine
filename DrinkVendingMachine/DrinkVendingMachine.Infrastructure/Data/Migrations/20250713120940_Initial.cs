using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrinkVendingMachine.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nominal = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    BrandId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drinks_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DrinkName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BrandName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PriceAtPurchase = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Coca-Cola" },
                    { 2, "Pepsi" },
                    { 3, "Fanta" },
                    { 4, "Sprite" },
                    { 5, "Mountain Dew" },
                    { 6, "7Up" }
                });

            migrationBuilder.InsertData(
                table: "Coins",
                columns: new[] { "Id", "Nominal", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 500 },
                    { 2, 2, 400 },
                    { 3, 5, 300 },
                    { 4, 10, 200 }
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "BrandId", "ImageUrl", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, "https://avatars.mds.yandex.net/i?id=e92740110da039f1caea551d11cf271a14e5291b-4261589-images-thumbs&n=13", "Coca-Cola Classic 0.33L", 82, 15 },
                    { 2, 2, "https://avatars.mds.yandex.net/i?id=0a2126d6b7a42414a80f9fe2a8d5d0fc6ae9bc20-12569871-images-thumbs&n=13", "Pepsi 0.33L", 80, 12 },
                    { 3, 3, "https://avatars.mds.yandex.net/i?id=ed98493dc133b26db2aae9a1dd732ee0eda8a7e9-5263755-images-thumbs&n=13", "Fanta Orange 0.33L", 84, 10 },
                    { 4, 4, "https://avatars.mds.yandex.net/i?id=c1bc9eca3c1bba813ec634c0b00b5a957313f00c-4462454-images-thumbs&n=13", "Sprite Lemon-Lime 0.33L", 75, 8 },
                    { 5, 5, "https://avatars.mds.yandex.net/i?id=37f21a1246146e0582ee6f20b26efae9ef8c57dd-5664541-images-thumbs&n=13", "Mountain Dew 0.33L", 85, 5 },
                    { 6, 6, "https://avatars.mds.yandex.net/i?id=dea8080e40047cb9a537383f0e33e92d14935301-8183104-images-thumbs&n=13", "7Up 0.33L", 72, 7 },
                    { 7, 1, "https://avatars.mds.yandex.net/i?id=2eeb95cb38c3725a08e95d446e52239ab1ccbc58-12928152-images-thumbs&n=13", "Coca-Cola Zero 0.33L", 83, 9 },
                    { 8, 2, "https://avatars.mds.yandex.net/i?id=bbd1651c52ac2f21a3b6375f0bf31e7562c3fd63-4219714-images-thumbs&n=13", "Pepsi Max 0.33L", 100, 6 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Drinks_BrandId",
                table: "Drinks",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
