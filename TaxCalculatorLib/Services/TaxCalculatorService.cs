using System;
using TaxCalculatorLib.Contracts;
using TaxCalculatorLib.Models;

namespace TaxCalculatorLib.Services
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
            
            // work out the amount taxable at the bracket rate, then work out amount and add to previous max
            var taxableAtBracketRate = grossIncome - (targetBracket.LowerIncomeBound-1);
            var finalAmount = targetBracket.PreviousBracketMax + taxableAtBracketRate * targetBracket.Percentage;
            
            // wrap result in calculation result object and return
            return TaxCalculationResult.CreateCalculationResult(finalAmount, targetBracket);
        }
    }
}
