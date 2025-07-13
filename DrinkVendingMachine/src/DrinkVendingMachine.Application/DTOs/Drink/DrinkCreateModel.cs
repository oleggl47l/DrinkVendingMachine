namespace DrinkVendingMachine.Application.DTOs.Drink;

public record DrinkCreateModel(string Name, int Price, int Quantity, string? ImageUrl, int BrandId);