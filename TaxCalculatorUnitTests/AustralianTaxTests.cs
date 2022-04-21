using Microsoft.VisualStudio.TestTools.UnitTesting;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Services;

namespace TaxCalculatorUnitTests
{
    /// <summary>
    /// These unit tests will run the app as the console application itself does - i.e. reading from the data source
    /// and validate that tax amounts match some expected values from the ATO calculator
    /// </summary>
    [TestClass]
    public class AustralianTaxTest
    {
        private readonly ITaxCalculator _taxCalculatorService;

        public AustralianTaxTest()
        {
            IData dataSource = new FileDataService();
            ITaxBrackets tb = new TaxBracketsService(dataSource);

            _taxCalculatorService = new TaxCalculatorService(tb);
        }
        
        // all expected values validated with tax calculator
        // https://www.ato.gov.au/Calculators-and-tools/Host/?anchor=STC&anchor=STC#STC/questions
        
        [TestMethod]
        public void TestTaxFreeFirstBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(17000);
            Assert.AreEqual(0,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestUpperEdgeTaxFreeBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(18200);
            Assert.AreEqual(0,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestLowerEdgeSecondBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(18201);
            Assert.AreEqual(0.19,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestMiddleSecondBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(30000);
            Assert.AreEqual(2242.00,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestUpperEdgeSecondBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(45000);
            Assert.AreEqual(5092,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestLowerEdgeThirdBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(45001);
            // interesting behaviour - calculated result is 5092.325 which would should round to 5092.33.
            // ATO calculator seems to round the result DOWN to .32. Will use .325 for unit test expected result
            // as .325 is correct, and rounding would be handled by the presentation layer.
            Assert.AreEqual(5092.325,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestMiddleThirdBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(80000);
            Assert.AreEqual(16467.00, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestUpperEdgeThirdBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(120000);
            Assert.AreEqual(29467, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestLowerEdgeFourthBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(120001);
            Assert.AreEqual(29467.37, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestMiddleFourthBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(150000);
            Assert.AreEqual(40567, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestUpperEdgeFourthBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(180000);
            Assert.AreEqual(51667, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestLowerEdgeFifthBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(180001);
            Assert.AreEqual(51667.45, bracketResult.Result);
        }
        
        [TestMethod]
        public void TestRandomHighIncomeFifthBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(450000);
            Assert.AreEqual(173167.00, bracketResult.Result);
        }
    }
}

