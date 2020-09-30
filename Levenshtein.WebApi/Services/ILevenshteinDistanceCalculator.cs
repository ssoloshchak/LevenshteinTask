using Levenshtein.WebApi.Models.Dto;

namespace Levenshtein.WebApi.Services
{
    public interface ILevenshteinDistanceCalculator
    {
        LevenshteinDistanceDto Calculate(string firstWord, string secondWord);
    }
}
