namespace DrinkVendingMachine.Application.DTOs.Drink;

public record DrinkModel(int Id, string Name, int Price, string? ImageUrl, int Quantity, string BrandName);