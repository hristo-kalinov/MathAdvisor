namespace MathAdvisor.PreAlgebra
{
    public static class CommonMath
    {
        public static string CalculateMean(params decimal[] parameters)
        {
            return $"{(parameters.Sum() / parameters.Length):F2}";
        }
    }
}
