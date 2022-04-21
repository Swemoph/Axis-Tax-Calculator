using System;
using System.Collections.Generic;
using System.Linq;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Models;

namespace TaxCalculatorLibrary.Services
{
    public class TaxBracketsService : ITaxBrackets
    {
        private List<TaxBracket> TaxBrackets { get; set; }

        public TaxBracketsService(IData dataService)
        {
            TaxBrackets = dataService.GetTaxBrackets();
        }
        
        public TaxBracket GetBracketForIncome(double grossIncome)
        {
            try
            {
                // get the correct bracket for incoming tax amount
                var targetBracket = TaxBrackets.SingleOrDefault(bracket =>
                    grossIncome >= bracket.LowerIncomeBound && grossIncome <= bracket.UpperIncomeBound);

                return targetBracket;
            }
            catch (InvalidOperationException) /* Catch exception if multiple brackets are found. */
            {
                // return null value which will cause calculation error.
                return null;
            }
        }
    }
}