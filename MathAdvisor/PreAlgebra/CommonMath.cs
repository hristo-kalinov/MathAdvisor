using Fractions;
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
    }
}
