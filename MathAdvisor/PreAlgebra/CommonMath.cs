using Fractions;
namespace MathAdvisor.PreAlgebra
{
    public static class CommonMath
    {
        /// <summary>
        /// Returns the Mean of any number of Fractions
        /// Example usage:
        /// Fraction num1 = new Fraction(5, 1); //5
        /// Fraction num1 = new Fraction(2, 3); //2/3
        /// Console.WriteLine(PreAlgebra.Mean(new Fraction[2] {num1, num2}));
        /// </summary>
        public static Fraction CalculateMean(Fraction[] numbers)
        {
            int sum = 0;
            foreach (int x in numbers)
            {
                sum += x;
            }
            Fraction aver = new Fraction(sum, numbers.Length);
            return aver;
        }

        /// <summary>
        /// Returns the Greate Common Divisor of any number of integers
        /// Examample usage:
        /// Console.WriteLine(PreAlgebra.GreatestCommonDivisor(new int[4] {8, 16, 32, 64}));
        /// </summary>
        public static int GreatestCommonDivisor(int[] numbers)
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

        private static int gcd(int a, int b)
        {
            if (a == 0)
                return b;
            return gcd(b % a, a);
        }
    }
}
