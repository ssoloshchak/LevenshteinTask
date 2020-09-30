using System;
using System.Linq;
using Levenshtein.WebApi.Models.Dto;
using Levenshtein.WebApi.Services;

namespace Levenshtein.WebApi.Infrastructure
{
    public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator
    {
        public LevenshteinDistanceDto Calculate(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;

            const int deletionCost = 1;
            const int insertionCost = 1;

            var tracer = new LevenshteinDistanceTracer(firstWord, secondWord);

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    //solving the elementary task: by deciding which way to take
                    var deletionTotalCost = tracer.Traces[i - 1, j].TotalCost + deletionCost;
                    var insertionTotalCost = tracer.Traces[i, j - 1].TotalCost + insertionCost;
                    var substitutionTotalCost = tracer.Traces[i - 1, j - 1].TotalCost + substitutionCost;

                    if (deletionTotalCost <= insertionTotalCost && deletionTotalCost <= substitutionTotalCost)
                    {
                        tracer.Delete(i, j, deletionTotalCost);
                    }
                    else if (insertionTotalCost <= deletionTotalCost && insertionTotalCost <= substitutionTotalCost)
                    {
                        tracer.Insert(i, j, insertionTotalCost);
                    }
                    else if (substitutionCost == 1)
                    {
                        tracer.Substitute(i, j, substitutionTotalCost);
                    }
                    else
                    {
                        tracer.Duplicate(i, j, substitutionTotalCost);
                    }
                }
            }

            return new LevenshteinDistanceDto
            {
                Distance = tracer.TraceResult.TotalCost,
                Trace = tracer.GetTracePath()
            };
        }

    }
}
