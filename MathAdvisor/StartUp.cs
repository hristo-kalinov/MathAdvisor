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
        public static string solution; //append part of the solution to this string
        static void Main(string[] args)
        {
            Solve("(x-2)*(x+7) = 0");
            Console.WriteLine(solution);
        }

        public static void Solve(string inputExpression)
        {
            solution = $"{inputExpression}\n";
            string formattedExpression = CommonMath.SymbolHandling(inputExpression);
            string leftSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[0]!.Trim();
            string rightSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[1]!.Trim();
            leftSide = CommonMath.CheckForParentheses(leftSide);
            rightSide = CommonMath.CheckForParentheses(rightSide);
            solution += $"Expand: {leftSide} = {rightSide}\n";
            string[] outputExpression = CommonMath.NormalizeEquation(leftSide, rightSide, false);
            leftSide = outputExpression[0];
            rightSide = outputExpression[1];

            solution += $"Transfer variables: {leftSide} = {rightSide}\n";
            string leftOutput = SymbolicExpression.Parse(leftSide).ToString();
            string rightOutput = SymbolicExpression.Parse(rightSide).ToString();

            solution += $"Simplify: {leftOutput} = {rightOutput}\n";
            string simplified = string.Concat(leftOutput, " = ", rightOutput);
            if (!leftOutput.Contains('^'))
            {
                LinearEquation.SolveLinear(leftOutput, rightOutput);
            }
            else
            {
                QuadraticEquation.SolveQuadratic(leftOutput, rightOutput);
            }
        }
    }

}