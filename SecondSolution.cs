using System;
using MathNet.Symbolics;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace MySolution
{
    public class SecondSolution
    {
        public static async Task Main()
        {
            string lettersFilter = "[a-z]";

            //Input in format: a*x + b - c = d*x + e OR a*x + b - c
            string[] inputExpressions = await File
                .ReadAllLinesAsync("equations.txt");
            for (int i = 0; i < inputExpressions.Length; i++)
            {
                string formattedExpression = Regex.Replace(inputExpressions[i], lettersFilter, "x");
                Console.Write($"{inputExpressions[i]}: ");
                string answer = $"Answer: " + SolveProblem(formattedExpression);
                Console.WriteLine(answer); //Output the solution
                await File.AppendAllTextAsync("../../../answers.txt", answer + Environment.NewLine);
            }
        }
        //Main method for solving problems
        private static string SolveProblem(string inputExpression) //Solve problem from input
        {
            if (inputExpression.Contains('=')) //Check if the problem is an equation or a simpler math problem
            {
                string leftSide = inputExpression.Split('=', StringSplitOptions.TrimEntries)[0]!.Trim();
                string rightSide = inputExpression.Split('=', StringSplitOptions.TrimEntries)[1]!.Trim();

                //Check for parentheses: 9 - 2*(x - 5) = x + 10 --> 19 - 2*x = 10 + x
                leftSide = CheckForParentheses(leftSide);
                rightSide = CheckForParentheses(rightSide);

                //Normalize the equation: a*x + b - c = d*x + e --> a*x - d*x = e - b + c (unknowns on the left, numbers on the right)
                string[] outputExpression = NormalizeEquation(leftSide, rightSide);
                leftSide = outputExpression[0];
                rightSide = outputExpression[1];

                string leftOutput = SymbolicExpression
                    .Parse(leftSide).ToString(); //Simplify left output as much as possible
                string rightOutput = SymbolicExpression
                    .Parse(rightSide).ToString(); //Simplify right output as much as possible
                string simplified = string.Concat(leftOutput, " = ", rightOutput);
                if (!leftOutput.Contains('^'))
                {
                    decimal leftCoefficient = CalculateCoefficient(leftOutput);
                    decimal rightCoefficient = CalculateCoefficient(rightOutput);
                    decimal result = rightCoefficient / leftCoefficient;
                    return $"{simplified} => x = {result:F2}"; //Final result
                }
                else
                {
                    return simplified; //These are quadratic equations so no implementation for now
                }
            }
            else
            {
                return SymbolicExpression.Parse(CheckForParentheses(inputExpression)).ToString(); //Solve the output as much as possible
            }
        }
        //Calculate coefficient of x and the right side to find the true value of x
        private static decimal CalculateCoefficient(string side)
        {
            bool coefficientHandled = false;
            decimal coefficient = 0m;
            if (side.Contains('/'))
            {
                decimal[] fraction = side.Split('/').Select(decimal.Parse).ToArray();
                coefficient = fraction[0] / fraction[1];
                coefficientHandled = true;
            }
            if (!coefficientHandled)
            {
                coefficient = decimal.Parse(side.Substring(0, side.Length - 2));
            }
            return coefficient;
        }
        //Method to check if either side of the equation has any parentheses to be expaned before solving
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
        //Method for normalizing equations if need be
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
        //Method for removal of unnecessary numbers and signs (sort of garbage collection for both sides)
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
        //Add plus or minus in front of the first number so the sign becomes known for the <currentSideSigns> array
        private static string AddSignInBeginning(string side)
        {
            if (!side.StartsWith('-'))
            {
                return string.Concat("+", side);
            }
            return side;
        }
        //Add all signs from the current side to the <currentSideSigns> array
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