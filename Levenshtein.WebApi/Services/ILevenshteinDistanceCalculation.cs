using System.Threading;
using System.Threading.Tasks;

namespace Levenshtein.WebApi.Services
{
    public interface ILevenshteinDistanceCalculation
    {
        int Calculate(string value, string value2);
        Task<int> CalculateAsync(string value, string value2, CancellationToken token);
    }
}
