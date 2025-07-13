namespace DrinkVendingMachine.Application.DTOs.Drink;

public record DrinkUpdateModel(int Id, string? Name, int? Price, int? Quantity, string? ImageUrl, int? BrandId);