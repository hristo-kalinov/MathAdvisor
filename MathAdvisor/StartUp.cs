using Fractions;
using MathAdvisor.PreAlgebra;
using MathAdvisor.Algebra;
using MathNet.Symbolics;
//using MathNet.Numerics;
using System.Text.RegularExpressions;
using MathNet.Symbolics;
namespace MathAdvisor
{
    /// <summary>
    /// Test environment for the application
    /// </summary>
    
    public class StartUp
    {
        static void Main(string[] args)
        {
            var equation = SymbolicExpression.Parse("x+3+4");
            Console.WriteLine(equation.ToString()); 
        }
    }

}