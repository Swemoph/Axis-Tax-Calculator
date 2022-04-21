using TaxCalculatorConsoleApp.Contracts;

namespace TaxCalculatorConsoleApp.Services
{
    public class ValidatorService : IValidator
    {
        public bool ValidateStringDouble(string inputDouble)
        {
            return double.TryParse(inputDouble, out var result) && result >= 0;
        }
    }
}