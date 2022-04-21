namespace TaxCalculatorLibrary.Models
{
    public class TaxCalculationResult
    {
        public double Result { get; }
        public bool WasError { get; }
        public TaxBracket BracketUsed { get; }

        private TaxCalculationResult(double result, bool wasError, TaxBracket bracketUsed)
        {
            Result = result;
            WasError = wasError;
            BracketUsed = bracketUsed;
        }

        public static TaxCalculationResult CreateCalculationResult(double result, TaxBracket bracketUsed)
        {
            return new TaxCalculationResult(result, false, bracketUsed);
        }

        public static TaxCalculationResult CreateErrorResult()
        {
            return new TaxCalculationResult(0, true, null);
        }
    }
}