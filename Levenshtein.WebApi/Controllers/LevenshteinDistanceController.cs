using System.Threading;
using System.Threading.Tasks;
using Levenshtein.WebApi.Models.Request;
using Levenshtein.WebApi.Models.Response;
using Levenshtein.WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Levenshtein.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LevenshteinDistanceController : ControllerBase
    {
        private readonly ILogger<LevenshteinDistanceController> _logger;
        private readonly ILevenshteinDistanceCalculator _distanceCalculator;

        public LevenshteinDistanceController(ILogger<LevenshteinDistanceController> logger, ILevenshteinDistanceCalculator distanceCalculator)
        {
            _logger = logger;
            _distanceCalculator = distanceCalculator;
        }

        [HttpGet]
        public ActionResult<GetLevenshtainDistanceResponse> Get([FromQuery] GetLevenshtainDistanceRequest request)
        {
            var response = new GetLevenshtainDistanceResponse()
            {
                FirstWord = request.FirstWord,
                SecondWord = request.SecondWord
            };
            response.Distance = _distanceCalculator.Calculate(request.FirstWord, request.SecondWord);
            return Ok(response);
        }
    }
}
