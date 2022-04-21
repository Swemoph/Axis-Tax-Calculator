namespace TaxCalculatorLibrary.Models
{
    public class TaxBracket
    {
        public double LowerIncomeBound { get; set; }
        public double UpperIncomeBound { get; set; }
        public double Percentage { get; set; }
        public double PreviousBracketMax { get; set; }
    }
}
