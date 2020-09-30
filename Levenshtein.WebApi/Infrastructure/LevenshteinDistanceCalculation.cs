﻿using System.Threading;
using System.Threading.Tasks;
using Levenshtein.WebApi.Services;

namespace Levenshtein.WebApi.Infrastructure
{
    public class LevenshteinDistanceCalculation: ILevenshteinDistanceCalculation
    {
        public int Calculate(string firstWord, string secondWord) => CalculateInternal(firstWord, secondWord);
        
        public async Task<int> CalculateAsync(string firstWord, string secondWord, 
            CancellationToken cancellationToken) => await Task.Factory.StartNew(() => CalculateInternal(firstWord, secondWord), cancellationToken);

        internal int CalculateInternal(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                matrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                matrixD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost,      
                        matrixD[i, j - 1] + insertionCost,         
                        matrixD[i - 1, j - 1] + substitutionCost);
                }
            }

            return matrixD[n - 1, m - 1];
        }

        private int Minimum(int a, int b, int c) => (a = a < b ? a : b) < c ? a : c;
    }

}
