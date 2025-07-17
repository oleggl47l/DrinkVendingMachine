namespace DrinkVendingMachine.Application.DTOs.Order;

public record OrderResultDto(Dictionary<int, int>? Change, int ChangeAmount);