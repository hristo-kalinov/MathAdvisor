using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApiTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSolutionController : ControllerBase
    {
        private readonly ILogger<GetSolutionController> _logger;

        public GetSolutionController(ILogger<GetSolutionController> logger)
        {
            _logger = logger;
        }

        [HttpPost (Name = "GetSolution")]
        public SolutionData Post([FromBody] string equation)
        {
            return new SolutionData
            {
                Date = DateTime.Now,
                solutionString = MathAdvisorLogic.Solver.Solve(equation)
            };
        }
    }
}