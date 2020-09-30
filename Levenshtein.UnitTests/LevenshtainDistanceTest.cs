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
        [InlineData("sik", "", 3, "sik->ik->k->")]
        public void DistanceShouldBeEqual(string firstWord, string secondWord, int expectedDistance, string expectedTrace)
        {
            var result = _calculator.Calculate(firstWord, secondWord);
            Assert.NotNull(result.Trace);
            Assert.Equal(expectedTrace, result.Trace);
            Assert.Equal(expectedDistance, result.Distance);
        }
    }
}
