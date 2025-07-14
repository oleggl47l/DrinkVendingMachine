namespace DrinkVendingMachine.Application.DTOs.Order;

public record OrderResultDto(string Message, Dictionary<int, int>? Change, int ChangeAmount);