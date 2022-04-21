using System;
using System.Globalization;
using TaxCalculatorConsoleApp.Contracts;
using TaxCalculatorConsoleApp.Services;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Services;

namespace TaxCalculatorConsoleApp
{
    class Program
    {
        private static ITaxCalculator TaxCalculator { get; set; }
        private static IValidator Validator { get; set; }
        private const string Culture = "en-au";
        
        public static void Main(string[] args)
        {
            SetupServices();

            var userInput = GetUserInput();

            // validate user input, if invalid tell the user and reprompt
            while (!Validator.ValidateStringDouble(userInput))
            {
                Console.WriteLine("Please enter a valid positive number");
                userInput = GetUserInput();
            }

            // once we know input is valid perform conversion
            var grossIncome = Convert.ToDouble(userInput);

            // Calculate tax payable
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
        
        private static void SetupServices()
        {
            IData dataSource = new FileDataService();
            ITaxBrackets tb = new TaxBracketsService(dataSource);
            TaxCalculator = new TaxCalculatorService(tb);
            Validator = new ValidatorService();
        }

        private static string FormatCurrency(double result)
        {
            return result.ToString("C", CultureInfo.GetCultureInfo(Culture));
        }
    }
}
