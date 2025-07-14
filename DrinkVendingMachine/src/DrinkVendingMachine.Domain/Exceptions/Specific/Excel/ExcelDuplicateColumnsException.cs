namespace DrinkVendingMachine.Domain.Exceptions.Specific.Excel;

public class ExcelDuplicateColumnsException(string[] duplicateColumns)
    : CustomException(new CustomExceptionArgument(nameof(duplicateColumns), duplicateColumns));