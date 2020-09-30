using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Levenshtein.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LevenshteinDistanceController : ControllerBase
    {
        private readonly ILogger<LevenshteinDistanceController> _logger;

        public LevenshteinDistanceController(ILogger<LevenshteinDistanceController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<int>> Get(CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
