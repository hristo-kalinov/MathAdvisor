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
            string leftSide = "(5)/(3)x+4=5-6";
            Console.WriteLine(SymbolicExpression.Parse("2^5"));
        }
    }

}