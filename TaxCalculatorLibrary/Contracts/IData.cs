using System.Collections.Generic;
using TaxCalculatorLib.Models;

namespace TaxCalculatorLib.Contracts
{
    /// <summary>
    /// Interface for accessing data.
    /// In this example, the only implementation accesses data from the embedded .json file
    /// However, for a real application, we could expect this to read from a database or use the EF data context
    /// </summary>
    public interface IData
    {
        List<TaxBracket> GetTaxBrackets();
    }
}