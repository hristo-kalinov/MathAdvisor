using Math_Advisor.API.Models;
using Microsoft.AspNetCore.Mvc;
using Math_Advisor.API.Services.MathAdvisorLogic.Algebra;
using Math_Advisor.API.Services.MathAdvisorLogic.PreAlgebra;
using Math_Advisor.API.Services.MathAdvisorLogic;
using System.Net;

namespace Math_Advisor.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class GetSolutionController : ControllerBase
	{
		[HttpOptions(Name = "GetSolution")]
		public HttpResponseMessage Options()
		{
			return new HttpResponseMessage { StatusCode = HttpStatusCode.OK };
		}
        [HttpPost]
		public SolutionModel Post([FromBody] string equation)
		{

			//HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
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