using Fractions;
using MathNet.Symbolics;
using System.Text;
using System.Text.RegularExpressions;

namespace Math_Advisor.API.Services.MathAdvisorLogic.PreAlgebra
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

        public static List<int> Factorize(int n)
        {
            var factors = new List<int>();
            while (n > 1)
            {
                for (int i = 2; 2 <= i && i <= n + 1; i++)
                {
                    if (n % i == 0)
                    {
                        n = int.Parse((n / i).ToString());
                        factors.Add(i);
                        break;
                    }
                }
            }
            return factors;
        }
        /// <summary>
        /// Method for normalizing equations if need be
        /// </summary>
        /// <param name="leftSide">Left side of the expression</param>
        /// <param name="rightSide">Right side of the expression</param>
        /// <returns>Normalized equation</returns>
        public static string[] NormalizeEquation(string leftSide, string rightSide, bool isQuadratic)
        {
            leftSide = StringHandling.AddSignInBeginning(leftSide);
            rightSide = StringHandling.AddSignInBeginning(rightSide);
            List<char> leftSideSigns = StringHandling.AddSignsInArray(leftSide);
            List<char> rightSideSigns = StringHandling.AddSignsInArray(rightSide);
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
                leftSideNumbers = StringHandling.RemoveRemainingNumbersAndSigns(leftSideNumbers, leftSideNumbersToBeRemoved);
                rightSideNumbers = StringHandling.RemoveRemainingNumbersAndSigns(rightSideNumbers, rightSideNumbersToBeRemoved);

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
            leftSideNumbers = StringHandling.RemoveRemainingNumbersAndSigns(leftSideNumbers, leftSideNumbersToBeRemoved);
            rightSideNumbers = StringHandling.RemoveRemainingNumbersAndSigns(rightSideNumbers, rightSideNumbersToBeRemoved);

            //Returning the final equation for the solution
            return new string[] { string.Join(" ", leftSideNumbers), string.Join(" ", rightSideNumbers) };
        }

        
    }
}
