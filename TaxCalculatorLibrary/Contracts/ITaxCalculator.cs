using TaxCalculatorLibrary.Models;

namespace TaxCalculatorLibrary.Contracts
{
    public interface ITaxCalculator
    {
        TaxCalculationResult CalculateAnnualTax (double grossIncome);
    }
}
