using System.Threading;
using System.Threading.Tasks;
using Levenshtein.WebApi.Services;

namespace Levenshtein.WebApi.Infrastructure
{
    public class LevenshteinDistanceCalculation: ILevenshteinDistanceCalculation
    {
        public int Calculate(string value, string value2) => CalculateInternal(value, value2);
        
        public async Task<int> CalculateAsync(string value, string value2, 
            CancellationToken cancellationToken) => await Task.Factory.StartNew(() => CalculateInternal(value, value2), cancellationToken);

        internal int CalculateInternal(string value, string value2)
        {
            return 1;
        }
    }
}
