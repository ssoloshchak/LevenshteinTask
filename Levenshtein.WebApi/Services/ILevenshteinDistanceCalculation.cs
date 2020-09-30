using System.Threading;
using System.Threading.Tasks;

namespace Levenshtein.WebApi.Services
{
    public interface ILevenshteinDistanceCalculation
    {
        int Calculate(string firstWord, string secondWord);
        Task<int> CalculateAsync(string firstWord, string secondWord, CancellationToken token);
    }
}
