using System.ComponentModel.DataAnnotations;

namespace Levenshtein.WebApi.Models.Request
{
    public class GetLevenshtainDistanceResponse
    {
        [Required]
        public string Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }
    }
}
