using MathNet.Symbolics;
using System.Text.RegularExpressions;

namespace MathAdvisor.Algebra
{
    /// <summary>
    /// Class for soling linear equations
    /// </summary>
    public static class LinearEquation
    {
        /// <summary>
        /// Main method for solving the equation
        /// </summary>
        /// <param name="inputExpression">Initial input expression</param>
        /// <returns>Solved equation for x</returns>
        public static void SolveLinear(string leftOutput, string rightOutput) //Solve problem from input
        {
            decimal result;
            object[] leftCoefficientOperation = CalculateCoefficient(leftOutput);
            decimal leftCoefficient = (decimal)leftCoefficientOperation[0];
            bool divisionNeeded = (bool)leftCoefficientOperation[1];
            decimal rightCoefficient = (decimal)CalculateCoefficient(rightOutput)[0];
            if (divisionNeeded)
            {
                result = leftCoefficient / rightCoefficient;
            }
            else
            {
                result = rightCoefficient / leftCoefficient;
            }
            StartUp.solution += $"x = {result}";
        }

        /// <summary>
        /// Calculate coefficient of x and the right side to find the true value of x
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Coefficient</returns>
        private static object[] CalculateCoefficient(string side)
        {
            bool divisionNeeded = false;
            decimal coefficient = 0m;
            if (side.Length == 1 && side.StartsWith('x'))
            {
                coefficient = 1m;
            }
            else
            {
                decimal[] fraction;
                if (side.Contains('x'))
                {
                    if (side.Contains('/') && side.Contains('*'))
                    {
                        fraction = side.Substring(0, side.Length - 2)
                        .Split('/', StringSplitOptions.TrimEntries)
                        .Select(decimal.Parse).ToArray();
                        coefficient = fraction[0] / fraction[1];
                    }
                    else if (side.Contains('/') && !side.Contains('*'))
                    {
                        coefficient = decimal.Parse(side.Substring(0, side.Length - 2));
                        divisionNeeded = true;
                    }
                    else if (!side.Contains('/') && side.Contains('*'))
                    {
                        coefficient = decimal.Parse(side.Substring(0, side.Length - 2));
                    }
                }
                else
                {
                    fraction = side
                        .Split('/', StringSplitOptions.TrimEntries)
                        .Select(decimal.Parse).ToArray();
                    if (side.Contains('/'))
                    {
                        coefficient = fraction[0] / fraction[1];
                    }
                    else
                    {
                        coefficient = fraction[0];
                    }
                }
            }
            return new object[] { coefficient, divisionNeeded };
        }
       
    }
}
