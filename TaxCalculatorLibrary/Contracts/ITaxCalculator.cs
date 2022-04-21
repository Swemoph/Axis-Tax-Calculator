using TaxCalculatorLib.Models;

namespace TaxCalculatorLib.Contracts
{
    public interface ITaxCalculator
    {
        TaxCalculationResult CalculateAnnualTax (double grossIncome);
    }
}
