namespace DrinkVendingMachine.Domain.Exceptions.Specific.Excel;

public class ExcelDataValidationException(string fieldName, int rowNumber, object invalidValue) : CustomException(
    new CustomExceptionArgument(nameof(fieldName), fieldName),
    new CustomExceptionArgument(nameof(rowNumber), rowNumber),
    new CustomExceptionArgument(nameof(invalidValue), invalidValue)
);