namespace DrinkVendingMachine.Domain.Exceptions.Specific.Excel;

public class ExcelBrandNotFoundException(int row, int brandId) 
    : CustomException(
        new CustomExceptionArgument(nameof(row), row),
        new CustomExceptionArgument(nameof(brandId), brandId));