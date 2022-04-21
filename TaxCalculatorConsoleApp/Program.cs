using System;
using System.Globalization;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Services;

namespace TaxCalculatorConsoleApp
{
    class Program
    {
        private static ITaxCalculator TaxCalculator { get; set; }
        private const string Culture = "en-au";
        
        public static void Main(string[] args)
        {
            SetupServices();

            var userInput = GetUserInput();

            while (!IsValidInput(userInput))
            {
                Console.WriteLine("Please enter a valid positive number");
                userInput = GetUserInput();
            }

            var grossIncome = Convert.ToDouble(userInput);

            var calculationResult = TaxCalculator.CalculateAnnualTax(grossIncome);

            Console.WriteLine(calculationResult.WasError
                ? "There was an error calculating your payable tax"
                : FormatCurrency(calculationResult.Result));
        }

        private static string GetUserInput()
        {
            Console.Write("Enter taxable income: $");
            var userInput = Console.ReadLine();
            return userInput;
        }

        private static bool IsValidInput(string input)
        {
            return double.TryParse(input, out var result) && result >= 0;
        }

        private static void SetupServices()
        {
            IData dataSource = new FileDataService();
            ITaxBrackets tb = new TaxBracketsService(dataSource);
            TaxCalculator = new TaxCalculatorService(tb);
        }

        private static string FormatCurrency(double result)
        {
            return result.ToString("C", CultureInfo.GetCultureInfo(Culture));
        }
    }
}
