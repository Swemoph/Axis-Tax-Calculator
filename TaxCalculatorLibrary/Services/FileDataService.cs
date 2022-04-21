using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TaxCalculatorLibrary.Contracts;
using TaxCalculatorLibrary.Models;

namespace TaxCalculatorLibrary.Services
{
    public class FileDataService : IData
    {
        /// <summary>
        /// Read from the embedded .json file, deserialize, and return tax bracket data
        /// </summary>
        /// <returns>List of tax brackets</returns>
        public List<TaxBracket> GetTaxBrackets()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(str => str.Contains("AustraliaBracketData.json"));

            var st = assembly.GetManifestResourceStream(resourceName);
            var streamReader = new StreamReader(st);

            var result = streamReader.ReadToEnd();

            return JsonConvert.DeserializeObject<List<TaxBracket>>(result);
        }
    }
}