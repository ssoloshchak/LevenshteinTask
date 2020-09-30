using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

            traceMatrix[0, 0] = new Trace(firstWord, Operation.None);

            for (var i = 1; i < n; i++)
            {
                matrixD[i, 0] = i;
                traceMatrix[i, 0] = new Trace(traceMatrix[i - 1, 0], Operation.Delete, 1, i);
                traceMatrix[i, 0].To.Remove(0, 1);
            }

            for (var j = 1; j < m; j++)
            {
                matrixD[0, j] = j;
                traceMatrix[0, j] = new Trace(traceMatrix[0, j - 1], Operation.Insert, 1, j);
                traceMatrix[0, j].To.Insert(0, secondWord[j - 1]);
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
                        traceMatrix[i, j] = new Trace(previous, Operation.Delete, deletionCost, deletionTotalCost);
                        traceMatrix[i, j].To.Remove(j, 1);
                    }
                    else if (insertionTotalCost <= deletionTotalCost && insertionTotalCost <= substitutionTotalCost)
                    {
                        matrixD[i, j] = insertionTotalCost;
                        var previous = traceMatrix[i, j - 1];
                        traceMatrix[i, j] = new Trace(previous, Operation.Insert, insertionCost, insertionTotalCost);
                        traceMatrix[i, j].To.Insert(i, secondWord[j - 1]);
                    }
                    else
                    {
                        matrixD[i, j] = substitutionTotalCost;
                        var previous = traceMatrix[i - 1, j - 1];
                        traceMatrix[i, j] = new Trace(previous, Operation.Update, substitutionCost, substitutionTotalCost);

                        if (substitutionCost > 0)
                            traceMatrix[i, j].To.Insert(j - 1, secondWord[j - 1]).Remove(j, 1);
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
            public Trace Previous { get; }
            public Operation Operation { get; }
            public string From { get; set; }
            public StringBuilder To { get; }

            public Trace(Trace previous, Operation operation, int currentCost, int totalCost)
            {
                Debug.Assert(previous != null);
                Operation = operation;
                Previous = previous;
                From = previous.To.ToString();
                To = new StringBuilder(From);
                CurrentCost = currentCost;
                TotalCost = totalCost;
            }

            public Trace(string from, Operation operation)
            {
                Operation = operation;
                From = from;
                To = new StringBuilder(From);
            }

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
