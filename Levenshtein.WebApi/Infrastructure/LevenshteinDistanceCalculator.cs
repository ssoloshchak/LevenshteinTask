using System;
using System.Collections.Generic;
using System.Linq;
using Levenshtein.WebApi.Services;

namespace Levenshtein.WebApi.Infrastructure
{
    public class LevenshteinDistanceCalculator : ILevenshteinDistanceCalculator
    {
        public int Calculate(string firstWord, string secondWord) => CalculateInternal(firstWord, secondWord);

        internal int CalculateInternal(string firstWord, string secondWord)
        {
            return ContainsWildCard(firstWord) || ContainsWildCard(secondWord)
                ? WildCardSupportImplementation(firstWord, secondWord)
                : ClassicalImplementation(firstWord, secondWord);
        }

        private int WildCardSupportImplementation(string firstWord, string secondWord)
        {
            throw new NotImplementedException();
        }

        private int ClassicalImplementation(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];
            var traceMatrix = new Trace[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            matrixD[0, 0] = 0;

            traceMatrix[0, 0] = new Trace
            {
                Operation = Operation.None,
                From = firstWord,
                To = firstWord,
                Previous = null,
                TotalCost = 0,
                CurrentCost = 0
            };

            for (var i = 1; i < n; i++)
            {
                matrixD[i, 0] = i;
                traceMatrix[i, 0] = new Trace
                {
                    TotalCost = i,
                    CurrentCost = 1,
                    Operation = Operation.Delete,
                    Previous = traceMatrix[i - 1, 0],
                    From = traceMatrix[i - 1, 0].To,
                    To = traceMatrix[i - 1, 0].To.Remove(0, 1)
                };
            }

            for (var j = 1; j < m; j++)
            {
                matrixD[0, j] = j;
                traceMatrix[0, j] = new Trace
                {
                    TotalCost = j,
                    CurrentCost = 1,
                    Operation = Operation.Insert,
                    Previous = traceMatrix[0, j - 1],
                    From = traceMatrix[0, j - 1].To,
                    To = traceMatrix[0, j - 1].To.Insert(0, secondWord[j - 1].ToString())
                };
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    //solving the elementary task: by deciding which way to take
                    var deletionTotalCost = matrixD[i - 1, j] + deletionCost;
                    var insertionTotalCost = matrixD[i, j - 1] + insertionCost;
                    var substitutionTotalCost = matrixD[i - 1, j - 1] + substitutionCost;

                    if (deletionTotalCost <= insertionTotalCost && deletionTotalCost <= substitutionTotalCost)
                    {
                        matrixD[i, j] = deletionTotalCost;
                        var previous = traceMatrix[i - 1, j];
                        traceMatrix[i, j] = new Trace
                        {
                            CurrentCost = deletionCost,
                            TotalCost = deletionTotalCost,
                            Previous = previous,
                            Operation = Operation.Delete,
                            From = previous.To,
                            To = previous.To.Remove(j, 1)
                        };
                    }
                    else if (insertionTotalCost <= deletionTotalCost && insertionTotalCost <= substitutionTotalCost)
                    {
                        matrixD[i, j] = insertionTotalCost;
                        var previous = traceMatrix[i, j - 1];
                        traceMatrix[i, j] = new Trace
                        {
                            CurrentCost = insertionCost,
                            TotalCost = insertionTotalCost,
                            Previous = previous,
                            Operation = Operation.Insert,
                            From = previous.To,
                            To = previous.To.Insert(i, secondWord[j - 1].ToString())
                        };
                    }
                    else
                    {
                        matrixD[i, j] = substitutionTotalCost;
                        var previous = traceMatrix[i - 1, j - 1];
                        traceMatrix[i, j] = new Trace
                        {
                            CurrentCost = substitutionCost,
                            TotalCost = substitutionTotalCost,
                            Previous = previous,
                            Operation = Operation.Update,
                            From = previous.To,
                            To = substitutionCost == 0 ? previous.To 
                                : previous.To.Insert(j - 1, secondWord[j - 1].ToString()).Remove(j, 1)

                        };
                    }
                }
            }

            var lastTrace = traceMatrix[n - 1, m - 1];
            var changeLog = GetChangeLog(lastTrace);

            return matrixD[n - 1, m - 1];
        }

        private List<string> GetChangeLog(Trace trace)
        {
            var changeLog = new Stack<string>();

            while (trace.Previous != null)
            {
                if (trace.CurrentCost > 0 && trace.Operation != Operation.None)
                    changeLog.Push(trace.ToString());

                trace = trace.Previous;
            }

            return changeLog.ToList();
        }
        public class Trace
        {
            public int CurrentCost { get; set; }
            public int TotalCost { get; set; }
            public Trace Previous { get; set; }
            public Operation Operation { get; set; }
            public string From { get; set; }
            public string To { get; set; }

            public override string ToString()
            {
                return $"{From} => {To} | {Enum.GetName(typeof(Operation), Operation)} | Cost: {CurrentCost} | Total cost: {TotalCost}";
            }
        }

        public enum Operation
        {
            None = 0,
            Insert = 1,
            Update = 2,
            Delete = 3
        }

        private bool ContainsWildCard(string value) => value.Any(t => t == '*' || t == '+');
    }

}
