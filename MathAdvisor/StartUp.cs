using Fractions;
using MathAdvisor.PreAlgebra;
using MathAdvisor.Algebra;
using MathNet.Symbolics;
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
            string solution = Solver.Solve("(x-1/2)*(x+1/2)=0");
            Console.WriteLine(solution);
        }
    }

}