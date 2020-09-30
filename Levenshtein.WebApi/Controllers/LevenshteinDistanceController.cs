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
        private readonly ILevenshteinDistanceCalculation _distanceCalculation;

        public LevenshteinDistanceController(ILogger<LevenshteinDistanceController> logger, ILevenshteinDistanceCalculation distanceCalculation)
        {
            _logger = logger;
            _distanceCalculation = distanceCalculation;
        }

        [HttpGet]
        public async Task<ActionResult<GetLevenshtainDistanceResponse>> Get([FromQuery] GetLevenshtainDistanceRequest request, CancellationToken ct)
        {
            var response = new GetLevenshtainDistanceResponse()
            {
                FirstWord = request.FirstWord,
                SecondWord = request.SecondWord
            };
            response.Distance = await _distanceCalculation.CalculateAsync(request.FirstWord, request.SecondWord, ct);
            return Ok(response);
        }
    }
}
