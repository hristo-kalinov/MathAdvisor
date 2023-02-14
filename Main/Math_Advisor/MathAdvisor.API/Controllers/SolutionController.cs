namespace Math_Advisor.API.Controllers
{
	using Math_Advisor.API.Models;
	using Microsoft.AspNetCore.Mvc;
	using Math_Advisor.API.Services.MathAdvisorLogic;
	using System.Net;
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

			equation = equation.Replace(",", ".");
			try
			{
				return new SolutionModel
				{
					Date = DateTime.Now,
					SolutionString = Solver.Solve(equation),
					Answers = Solver.answers,
					Success = true
				};
			}
			catch(Exception e)
            {
				Console.WriteLine(e);
				return new SolutionModel
				{
					Date = DateTime.Now,
					Success = false
				};
			}
		}
	}
}