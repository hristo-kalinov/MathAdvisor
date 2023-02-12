using Math_Advisor.API.Models;
using Microsoft.AspNetCore.Mvc;
using Math_Advisor.API.Services.MathAdvisorLogic.Algebra;
using Math_Advisor.API.Services.MathAdvisorLogic.PreAlgebra;
using Math_Advisor.API.Services.MathAdvisorLogic;

namespace Math_Advisor.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetSolutionController : ControllerBase
	{
		[HttpPost(Name = "GetSolution")]
		public SolutionModel Post([FromBody] string equation)
		{
			return new SolutionModel
			{
				Date = DateTime.Now,
				SolutionString = Solver.Solve(equation),
				Answers = Solver.answers,
				Success = true
			};
		}
	}
}