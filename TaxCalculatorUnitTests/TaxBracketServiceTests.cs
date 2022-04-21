using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Models;
using TaxCalculatorLibrary.Services;

namespace TaxCalculatorUnitTests
{
    [TestClass]
    public class TaxBracketServiceTests
    {
        private readonly ITaxBrackets _taxBracketsService;

        public TaxBracketServiceTests()
        {
            var dataService = GetDataMock();
            _taxBracketsService = new TaxBracketsService(dataService);
        }

        private static IData GetDataMock()
        {
            var mockDataService = new Mock<IData>();
            var mockBrackets = new List<TaxBracket>
            {
                new()
                {
                    LowerIncomeBound = 0,
                    UpperIncomeBound = 100,
                    Percentage = 0.0,
                    PreviousBracketMax = 0
                },
                new()
                {
                    LowerIncomeBound = 101,
                    UpperIncomeBound = 200,
                    Percentage = 0.15,
                    PreviousBracketMax = 10
                },
                new()
                {
                    LowerIncomeBound = 201,
                    UpperIncomeBound = 300,
                    Percentage = 0.20,
                    PreviousBracketMax = 30
                },
                new() /* BAD DATA - Overlapping bound with previous bracket */
                {
                    LowerIncomeBound = 250,
                    UpperIncomeBound = 300,
                    Percentage = 0.40,
                    PreviousBracketMax = 45
                }
            };

            mockDataService.Setup(ds => ds.GetTaxBrackets()).Returns(mockBrackets);
            return mockDataService.Object;
        }


        [TestMethod]
        public void TestMiddleOfLowerBracket()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(50);
            Assert.AreEqual(0, bracketResult.LowerIncomeBound);
            Assert.AreEqual(100, bracketResult.UpperIncomeBound);
        }
        
        [TestMethod]
        public void TestMiddleOfMiddleBracket()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(210);
            Assert.AreEqual(201, bracketResult.LowerIncomeBound);
            Assert.AreEqual(300, bracketResult.UpperIncomeBound);
        }
        
        [TestMethod]
        public void TestOnBracketUpperBound()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(100);
            Assert.AreEqual(0, bracketResult.LowerIncomeBound);
            Assert.AreEqual(100, bracketResult.UpperIncomeBound);
        }
        
        [TestMethod]
        public void TestOnBracketLowerBound()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(101);
            Assert.AreEqual(101, bracketResult.LowerIncomeBound);
            Assert.AreEqual(200, bracketResult.UpperIncomeBound);
        }
        
        [TestMethod]
        public void TestNoExceptionOnInvalidValue()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(-1);
            Assert.IsNull(bracketResult);
        }
        
        [TestMethod]
        public void TestNoExceptionOnOverlappingBound()
        {
            var bracketResult = _taxBracketsService.GetBracketForIncome(270);
            Assert.IsNull(bracketResult);
        }
    }
}