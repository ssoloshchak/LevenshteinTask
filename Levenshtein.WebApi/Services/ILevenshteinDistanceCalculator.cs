namespace Levenshtein.WebApi.Services
{
    public interface ILevenshteinDistanceCalculator
    {
        int Calculate(string firstWord, string secondWord);
    }
}
