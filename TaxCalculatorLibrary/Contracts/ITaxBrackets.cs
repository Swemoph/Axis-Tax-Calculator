using TaxCalculatorLibrary.Models;

namespace TaxCalculatorLibrary.Contracts
{
    public interface ITaxBrackets
    {
        TaxBracket GetBracketForIncome(double grossIncome);
    }
}