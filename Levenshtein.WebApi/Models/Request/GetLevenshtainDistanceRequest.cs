using System.ComponentModel.DataAnnotations;

namespace Levenshtein.WebApi.Models.Request
{
    public class GetLevenshtainDistanceRequest
    {
        [Required]
        public string Value1 { get; set; }

        [Required]
        public string Value2 { get; set; }
    }
}
