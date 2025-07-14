
namespace DrinkVendingMachine.Domain.Exceptions.Specific.Excel;

public class ExcelMissingColumnsException(string[] missingColumns) 
    : CustomException(new CustomExceptionArgument(nameof(missingColumns), missingColumns));