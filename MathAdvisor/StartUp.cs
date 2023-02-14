using Fractions;
using MathAdvisor.PreAlgebra;
using MathAdvisor.Algebra;
using MathNet.Symbolics;
//using MathNet.Numerics;
using System.Text.RegularExpressions;
namespace MathAdvisor
{
	/// <summary>
	/// Test environment for the application
	/// </summary>

	public class StartUp
	{
		static void Main(string[] args)
		{
			Console.WriteLine(Solver.Solve("x*2.5=3"));
		}
	}
}