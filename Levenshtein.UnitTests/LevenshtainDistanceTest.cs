using Levenshtein.WebApi.Infrastructure;
using Levenshtein.WebApi.Services;
using Xunit;

namespace Levenshtein.UnitTests
{
    public class LevenshtainDistanceTest
    {
        private readonly ILevenshteinDistanceCalculator _calculator = new LevenshteinDistanceCalculator();

        [Theory]
        //   [InlineData("cat", "flat", 2)]
        //[InlineData("flat", "cat", 2)]
        [InlineData("sik", "sok", 1)]
        //[InlineData("*ike", "trike", 0)]
        //[InlineData("*ike", "bbike ", 0)]
        //[InlineData("bike", "*ike", 0)]
        //[InlineData("bike", "*bike", 0)]
        //[InlineData("+ike", "bike", 0)]
        //[InlineData("bike", "+bike", 1)]
        public void DistanceShouldBeEqual(string firstWord, string secondWord, int expectedDistance)
        {
            int calculatedDistance = _calculator.Calculate(firstWord, secondWord);
            Assert.Equal(expectedDistance, calculatedDistance);
        }
    }
}
