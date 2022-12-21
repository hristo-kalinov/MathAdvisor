using Fractions;
using MathNet.Symbolics;
using System.Text;
using System.Text.RegularExpressions;

namespace MathAdvisor.PreAlgebra
{
    /// <summary>
    /// Class for implementing common math tasks
    /// </summary>
    public static class CommonMath
    {
        /// <summary>
        /// Calculate Mean of any numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>Average value of input numbers</returns>
        public static Fraction CalculateMean(params Fraction[] numbers)
        {
            int sum = 0;
            foreach (int x in numbers)
            {
                sum += x;
            }
            Fraction avg = new Fraction(sum, numbers.Length);
            return avg;
        }

        /// <summary>
        /// Calculate greatest common divisor for any numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>Greatest common divisor</returns>
        public static int CalculateGreatestCommonDivisor(int[] numbers)
        {
            int result = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                result = gcd(numbers[i], result);

                if (result == 1)
                {
                    return 1;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculate least common multiple for any numbers
        /// </summary>
        /// <param name="numbers"></param>
        /// <returns>Least common multiple</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int CalculateLeastCommonMultiple(int[] numbers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calculate prime factors for any number
        /// </summary>
        /// <param name="number"></param>
        /// <returns>Prime factors</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static int[] CalculatePrimeFactors(int number)
        {
            throw new NotImplementedException();
        }

        private static int gcd(int a, int b)
        {
            if (a == 0)
                return b;
            return gcd(b % a, a);
        }

        /// <summary>
        /// Method to check if either side of the equation has any parentheses to be expaned before solving
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Expaned expression without parentheses</returns>

        public static string CheckForParentheses(string side)
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
        public static string[] NormalizeEquation(string leftSide, string rightSide, bool isQuadratic)
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

            if (isQuadratic) //if the equation is quadratic add everything to the left: x^2 + 5*x + 4 = 0
            {
                for (int num = 0; num < initialLeftSideNumbersCount; num++)
                {
                    if (num != 0 || leftSideSigns[0] == '-')
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
                leftSideNumbers = RemoveRemainingNumbersAndSigns(leftSideNumbers, leftSideNumbersToBeRemoved);
                rightSideNumbers = RemoveRemainingNumbersAndSigns(rightSideNumbers, rightSideNumbersToBeRemoved);

                //Returning the final equation for the solution
                return new string[] { string.Join(" ", leftSideNumbers), string.Join(" ", rightSideNumbers) };
            }
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
        /// Add plus or minus in front of the first number so the sign becomes known for the <currentSideSigns> array
        /// </summary>
        /// <param name="side">Current side we are working with</param>
        /// <returns>Expression with an added sign in the beginning</returns>
        /// 

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

        public static string SymbolHandling(string input)
        {
            string lettersFilter = "[a-z]";
            string formattedExpression = Regex.Replace(input, lettersFilter, "x");
            return formattedExpression;
        }

        public static string ReverseEquation(string input)//3 + x + x^2 becomes x^2 + x + 3
        {
            input = HandlePlusesAndMinuses(input);
            input = string.Join(" ", input.ToString().Split(' ').Reverse());
            var builder = new StringBuilder();
            int count = 0;
            foreach (var c in input)
            {
                builder.Append(c);
                if (c == '-' || c == '+' && count != 0)
                {
                    builder.Append(' ');
                }
                count++;
            }
            input = builder.ToString();
            if (input[0] == '+')
            {
                input = input.Remove(0, 1);
            }
            return input;

        }

        public static string HandlePlusesAndMinuses(string input)
        {
            if (input[0] != '-')
            {
                input = $"+{input}";
            }
            for (int i = input.IndexOf('+'); i > -1; i = input.IndexOf('+', i + 1))
            {
                if (input[i + 1] == ' ')
                {
                    input = input.Remove(i + 1, 1);
                }
            }
            for (int i = input.IndexOf('-'); i > -1; i = input.IndexOf('-', i + 1))
            {
                if (input[i + 1] == ' ')
                {
                    input = input.Remove(i + 1, 1);
                }
            }
            return input;
        }
    }
}
