namespace Levenshtein.WebApi.Models.Response
{
    public class GetLevenshtainDistanceResponse
    {
        public string FirstWord { get; set; }

        public string SecondWord { get; set; }

        public int Distance { get; set; }
    }
}
