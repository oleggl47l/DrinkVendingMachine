namespace DrinkVendingMachine.Application.DTOs.Drink;

public record DrinkExcelImportModel(
    string Name,
    int BrandId,
    int Price,
    int Quantity,
    string? ImageUrl
);