using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Models;

namespace TaxCalculatorLibrary.Services
{
    public class TaxCalculatorService : ITaxCalculator
    {
        private readonly ITaxBrackets _taxBracketsService;
        
        public TaxCalculatorService(ITaxBrackets taxBrackets)
        {
            _taxBracketsService = taxBrackets;
        }

        public TaxCalculationResult CalculateAnnualTax(double grossIncome)
        {
            // get bracket for the target income, and if null return error
            var targetBracket = _taxBracketsService.GetBracketForIncome(grossIncome);
            if (targetBracket == null) return TaxCalculationResult.CreateErrorResult();

            /* This minus one logic is to deal with non-continuous bands. Lets have a chat around
             why this is here. */
            var lowerBound = targetBracket.LowerIncomeBound == 0
                ? targetBracket.LowerIncomeBound
                : targetBracket.LowerIncomeBound - 1;
            
            // work out the amount taxable at the bracket rate, then work out amount and add to previous max
            var taxableAtBracketRate = grossIncome - lowerBound;
            var finalAmount = targetBracket.PreviousBracketMax + taxableAtBracketRate * targetBracket.Percentage;
            
            // wrap result in calculation result object and return
            return TaxCalculationResult.CreateCalculationResult(finalAmount, targetBracket);
        }
    }
}
