using System.ComponentModel.DataAnnotations;

namespace Levenshtein.WebApi.Models.Request
{
    public class GetLevenshtainDistanceRequest
    {
        [Required]
        public string FirstWord { get; set; }

        [Required]
        public string SecondWord { get; set; }
    }
}
