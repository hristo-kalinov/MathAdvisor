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
        public static string Solve(string inputExpression) //Solve problem from input
        {
            string lettersFilter = "[a-z]";
            string formattedExpression = Regex.Replace(inputExpression, lettersFilter, "x");
            string leftSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[0]!.Trim();
            string rightSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[1]!.Trim();

            leftSide = CheckForParentheses(leftSide);
            rightSide = CheckForParentheses(rightSide);

            string[] outputExpression = NormalizeEquation(leftSide, rightSide);
            leftSide = outputExpression[0];
            rightSide = outputExpression[1];

            string leftOutput = SymbolicExpression.Parse(leftSide).ToString();
            string rightOutput = SymbolicExpression.Parse(rightSide).ToString();
            string simplified = string.Concat(leftOutput, " = ", rightOutput);
            if (!leftOutput.Contains('^'))
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
                return $"{simplified} => x = {result:F2}";
            }
            else
            {
                return simplified; //These are quadratic equations so no implementation for now
            }
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

        /// <summary>
        /// Method to check if either side of the equation has any parentheses to be expaned before solving
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Expaned expression without parentheses</returns>
        private static string CheckForParentheses(string side)
        {
            if (side.Contains('(') && side.Contains(')'))
            {
                Expression expression = Infix.ParseOrThrow(side);
                Expression expanded = Algebraic.Expand(expression);
                side = Infix.Format(expanded);
            }
            return side;
        }

        /// <summary>
        /// Method for normalizing equations if need be
        /// </summary>
        /// <param name="leftSide">Left side of the expression</param>
        /// <param name="rightSide">Right side of the expression</param>
        /// <returns>Normalized equation</returns>
        private static string[] NormalizeEquation(string leftSide, string rightSide)
        {
            leftSide = AddSignInBeginning(leftSide);
            rightSide = AddSignInBeginning(rightSide);
            List<char> leftSideSigns = AddSignsInArray(leftSide);
            List<char> rightSideSigns = AddSignsInArray(rightSide);
            List<string> leftSideNumbers = leftSide.Split(new string[] { "+", "-", " + ", " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> rightSideNumbers = rightSide.Split(new string[] { "+", "-", " + ", " - " }, StringSplitOptions.RemoveEmptyEntries).ToList();
            List<string> leftSideNumbersToBeRemoved = new();
            List<string> rightSideNumbersToBeRemoved = new();
            int initialLeftSideNumbersCount = leftSideNumbers.Count;
            int initialRightSideNumbersCount = rightSideNumbers.Count;
            for (int num = 0; num < initialLeftSideNumbersCount; num++)
            {
                if (!leftSideNumbers[num].Contains('x'))
                {
                    if (leftSideSigns[num] == '+')
                    {
                        rightSideNumbers.Add(string.Concat("- ", leftSideNumbers[num]));
                        leftSideNumbersToBeRemoved.Add(leftSideNumbers[num]);
                    }
                    else
                    {
                        rightSideNumbers.Add(string.Concat("+ ", leftSideNumbers[num]));
                        leftSideNumbersToBeRemoved.Add(leftSideNumbers[num]);
                    }
                }
                else if (num != 0 || leftSideSigns[0] == '-')
                {
                    if (leftSideSigns[num] == '+')
                    {
                        leftSideNumbers[num] = string.Concat("+ ", leftSideNumbers[num]);
                    }
                    else
                    {
                        if (num == 0)
                        {
                            leftSideNumbers[num] = string.Concat("-", leftSideNumbers[num]);
                        }
                        else
                        {
                            leftSideNumbers[num] = string.Concat("- ", leftSideNumbers[num]);
                        }
                    }
                }
            }
            for (int num = 0; num < initialRightSideNumbersCount; num++)
            {
                if (rightSideNumbers[num].Contains('x'))
                {
                    if (rightSideSigns[num] == '+')
                    {
                        leftSideNumbers.Add(string.Concat("- ", rightSideNumbers[num]));
                        rightSideNumbersToBeRemoved.Add(rightSideNumbers[num]);
                    }
                    else
                    {
                        leftSideNumbers.Add(string.Concat("+ ", rightSideNumbers[num]));
                        rightSideNumbersToBeRemoved.Add(rightSideNumbers[num]);
                    }
                }
                else if (num != 0 || rightSideSigns[0] == '-')
                {
                    if (rightSideSigns[num] == '+')
                    {
                        rightSideNumbers[num] = string.Concat("+ ", rightSideNumbers[num]);
                    }
                    else
                    {
                        if (num == 0)
                        {
                            rightSideNumbers[num] = string.Concat("-", rightSideNumbers[num]);
                        }
                        else
                        {
                            rightSideNumbers[num] = string.Concat("- ", rightSideNumbers[num]);
                        }
                    }
                }
            }
            leftSideNumbers = RemoveRemainingNumbersAndSigns(leftSideNumbers, leftSideNumbersToBeRemoved);
            rightSideNumbers = RemoveRemainingNumbersAndSigns(rightSideNumbers, rightSideNumbersToBeRemoved);

            //Returning the final equation for the solution
            return new string[] { string.Join(" ", leftSideNumbers), string.Join(" ", rightSideNumbers) };
        }

        /// <summary>
        /// Method for removal of unnecessary numbers and signs
        /// </summary>
        /// <param name="currentSideNumbers">Current numbers of the side we are working with</param>
        /// <param name="currentSideNumbersToBeRemoved">Current numbers to be removed from the side we are workig with</param>
        /// <returns>Expression without unnecessary numbers and signs</returns>
        private static List<string> RemoveRemainingNumbersAndSigns(List<string> currentSideNumbers, List<string> currentSideNumbersToBeRemoved)
        {
            for (int i = 0; i < currentSideNumbersToBeRemoved.Count; i++)
            {
                if (currentSideNumbers.Contains(currentSideNumbersToBeRemoved[i]))
                {
                    currentSideNumbers.Remove(currentSideNumbersToBeRemoved[i]);
                }
            }
            for (int i = 0; i < currentSideNumbers.Count; i++)
            {
                if (currentSideNumbers[0].Contains(' ')
                    || currentSideNumbers[0].Contains('+'))
                {
                    currentSideNumbers[0] = currentSideNumbers[0].Replace('+', ' ').Trim();
                }
            }
            if (currentSideNumbers.Count == 0)
            {
                currentSideNumbers.Add("0");
            }
            return currentSideNumbers;
        }

        /// <summary>
        /// Add plus or minus in front of the first number so the sign becomes known for the <currentSideSigns> array
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Expression with an added sign in the beginning</returns>
        private static string AddSignInBeginning(string side)
        {
            if (!side.StartsWith('-'))
            {
                return string.Concat("+", side);
            }
            return side;
        }

        /// <summary>
        /// Add all signs from the current side to the <currentSideSigns> array
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Array that contains all signs of an expression</returns>
        private static List<char> AddSignsInArray(string side)
        {
            List<char> array = new();
            for (int i = 0; i < side.Length; i++)
            {
                if (side[i] == '+' || side[i] == '-')
                {
                    array.Add(side[i]);
                }
            }
            return array;
        }
    }
}
