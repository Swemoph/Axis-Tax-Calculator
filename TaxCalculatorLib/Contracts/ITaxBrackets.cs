using TaxCalculatorLib.Models;

namespace TaxCalculatorLib.Contracts
{
    public interface ITaxBrackets
    {
        TaxBracket GetBracketForIncome(double grossIncome);
    }
}