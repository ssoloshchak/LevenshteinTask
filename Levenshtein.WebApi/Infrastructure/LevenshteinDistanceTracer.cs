using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Levenshtein.WebApi.Infrastructure
{
    public class LevenshteinDistanceTracer
    {
        public Trace[,] Traces { get; }
        public string FirstWord { get; }
        public string SecondWord { get; }

        public LevenshteinDistanceTracer(string firstWord, string secondWord)
        {
            FirstWord = firstWord;
            SecondWord = secondWord;

            int n = firstWord.Length + 1;
            int m = secondWord.Length + 1;

            Traces = new Trace[n, m];

            Traces[0, 0] = new Trace(FirstWord, Operation.None);

            for (var i = 1; i < n; i++)
            {
                Traces[i, 0] = new Trace(Traces[i - 1, 0], Operation.Delete, 1, i);
                Traces[i, 0].To.Remove(0, 1);
            }

            for (var j = 1; j < m; j++)
            {
                Traces[0, j] = new Trace(Traces[0, j - 1], Operation.Insert, 1, j);
                Traces[0, j].To.Insert(0, SecondWord[j - 1]);
            }
        }

        public Trace TraceResult => Traces[FirstWord.Length, SecondWord.Length];

        public string GetTracePath()
        {
            var fullTrace = TraceResult.Reverse().ToList();
            return new StringBuilder(fullTrace.First().From)
                .Append("->")
                .AppendJoin("->", fullTrace.Where(item => item.Operation != Operation.None).Select(item => item.To))
                .ToString();
        }

        public void Delete(int i, int j, int deletionCost, int deletionTotalCost)
        {
            var previous = Traces[i - 1, j];
            Traces[i, j] = new Trace(previous, Operation.Delete, deletionCost, deletionTotalCost);
            if (deletionCost != 0)
                Traces[i, j].To.Remove(j, 1);
        }

        public void Insert(int i, int j, int insertionCost, int insertionTotalCost)
        {
            var previous = Traces[i, j - 1];
            Traces[i, j] = new Trace(previous, Operation.Insert, insertionCost, insertionTotalCost);
            if (insertionCost != 0 || SecondWord[j - 1] == '*' || SecondWord[j - 1] == '+')
                Traces[i, j].To.Insert(i, SecondWord[j - 1]);
        }

        public void Substitute(int i, int j, int substitutionCost, int substitutionTotalCost)
        {
            var previous = Traces[i - 1, j - 1];
            Traces[i, j] = new Trace(previous, Operation.Substitute, substitutionCost, substitutionTotalCost);
            if (substitutionCost != 0 || SecondWord[j - 1] == '*' || SecondWord[j - 1] == '+')
                Traces[i, j].To.Insert(j - 1, SecondWord[j - 1]).Remove(j, 1);
        }

        public void Duplicate(int i, int j, int substitutionTotalCost)
        {
            var previous = Traces[i - 1, j - 1];
            Traces[i, j] = new Trace(previous, Operation.None, 0, substitutionTotalCost);
        }

        public enum Operation
        {
            None = 0,
            Insert = 1,
            Substitute = 2,
            Delete = 3
        }

        public class Trace : IEnumerable<Trace>
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

            public IEnumerator<Trace> GetEnumerator()
            {
                var trace = this;
                while (trace.Previous != null)
                {
                    yield return trace;

                    trace = trace.Previous;
                }
            }

            //for debug purposes
            public override string ToString()
            {
                return
                    $"{From} => {To} | {Enum.GetName(typeof(Operation), Operation)} | Cost: {CurrentCost} | Total cost: {TotalCost}";
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}