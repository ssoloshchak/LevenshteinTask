using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Levenshtein.WebApi.Models.Dto
{
    public class LevenshteinDistanceDto
    {
        public int Distance { get; set; }

        public string Trace { get; set; } 
    }
}
