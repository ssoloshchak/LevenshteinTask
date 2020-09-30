using Levenshtein.WebApi.Infrastructure;
using Levenshtein.WebApi.Services;
using Xunit;

namespace Levenshtein.UnitTests
{
    public class LevenshtainDistanceTest
    {
        private readonly ILevenshteinDistanceCalculator _calculator = new LevenshteinDistanceCalculator();

        [Theory]
        [InlineData("cat", "flat", 2, "cat->fat->flat")]
        [InlineData("abcd", "efgh", 4, "abcd->ebcd->efcd->efgd->efgh")]
        [InlineData("flat", "cat", 2, "flat->clat->cat")]
        [InlineData("sik", "sok", 1, "sik->sok")]
        [InlineData("sik", "sk", 1, "sik->sk")]
        //[InlineData("*ike", "trike", 0)]
        //[InlineData("*ike", "bbike ", 0)]
        //[InlineData("bike", "*ike", 0)]
        //[InlineData("bike", "*bike", 0)]
        //[InlineData("+ike", "bike", 0)]
        //[InlineData("bike", "+bike", 1)]
        public void DistanceShouldBeEqual(string firstWord, string secondWord, int expectedDistance, string expectedTrace)
        {
            var result = _calculator.Calculate(firstWord, secondWord);
            Assert.Equal(expectedDistance, result.Distance);
            Assert.True(string.Equals(expectedTrace, result.Trace));
        }
    }
}
