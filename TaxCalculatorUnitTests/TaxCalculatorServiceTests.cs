using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Models;
using TaxCalculatorLibrary.Services;

namespace TaxCalculatorUnitTests
{
    
    [TestClass]
    public class TaxCalculatorServiceTests
    {
        private readonly ITaxCalculator _taxCalculatorService;

        public TaxCalculatorServiceTests()
        {
            var bracketsService = GetBracketsMock();
            _taxCalculatorService = new TaxCalculatorService(bracketsService);
        }

        private static ITaxBrackets GetBracketsMock()
        {
            var b1 = new TaxBracket()
            {
                LowerIncomeBound = 0,
                UpperIncomeBound = 100,
                Percentage = 0,
                PreviousBracketMax = 0
            };
            
            var b2 = new TaxBracket()
            {
                LowerIncomeBound = 101,
                UpperIncomeBound = 200,
                Percentage = 0.15,
                PreviousBracketMax = 0
            };

            var b3 = new TaxBracket()
            {
                LowerIncomeBound = 201,
                UpperIncomeBound = 300,
                Percentage = 0.20,
                PreviousBracketMax = 15
            };
            
            var mockBracketService = new Mock<ITaxBrackets>();

            mockBracketService.Setup(bracketService =>
                bracketService.GetBracketForIncome(50)).Returns(b1);
            
            mockBracketService.Setup(bracketService =>
                bracketService.GetBracketForIncome(100)).Returns(b1);
            
            mockBracketService.Setup(bracketService =>
                bracketService.GetBracketForIncome(101)).Returns(b2);
            
            mockBracketService.Setup(bracketService =>
                bracketService.GetBracketForIncome(150)).Returns(b2);
            
            mockBracketService.Setup(bracketService =>
                bracketService.GetBracketForIncome(250)).Returns(b2);
            
            return mockBracketService.Object;
        }
        
        [TestMethod]
        public void TestTaxFreeFirstBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(50);
            Assert.AreEqual(0,bracketResult.Result);
        }
        
        [TestMethod]
        public void Test15PercentSecondBracket()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(150);
            Assert.AreEqual(7.5,bracketResult.Result);
        }
        
        [TestMethod]
        public void TestOutOfBoundsNegativeReturnsError()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(-100);
            Assert.IsTrue(bracketResult.WasError);
        }
        
        [TestMethod]
        public void TestOutOfBoundsReturnsError()
        {
            var bracketResult = _taxCalculatorService.CalculateAnnualTax(1000000);
            Assert.IsTrue(bracketResult.WasError);
        }
    }
}

