﻿namespace Math_Advisor.API.Services.MathAdvisorLogic.Algebra
{
	// Class for soling linear equations
	public static class LinearEquation
	{
		// Main method for solving the equation
		public static void SolveLinear(string leftOutput, string rightOutput) //Solve problem from input
		{
			decimal result = 0m;
			object[] leftCoefficientOperation = CalculateCoefficient(leftOutput);
			decimal leftCoefficient = (decimal)leftCoefficientOperation[0];
			bool divisionNeeded = (bool)leftCoefficientOperation[1];
			bool multiplicationNeeded = (bool)leftCoefficientOperation[2];
			bool reverseSignNeeded = (bool)leftCoefficientOperation[3];
			decimal rightCoefficient = (decimal)CalculateCoefficient(rightOutput)[0];
			if (reverseSignNeeded)
			{
				rightCoefficient *= -1;
				result = rightCoefficient;
			}
			if (divisionNeeded)
			{
				result = rightCoefficient / leftCoefficient;
			}
			else if (multiplicationNeeded)
			{
				result = rightCoefficient * leftCoefficient;
			}
			Solver.answers.Add(result.ToString().Replace(",", "."));
		}

		// Calculate coefficient of x and the right side to find the true value of x
		private static object[] CalculateCoefficient(string side)
		{
			bool divisionNeeded = false;
			bool multiplicationNeeded = false;
			bool reverseSignNeeded = false;
			decimal coefficient = 0m;
			if (side.Length == 1 && side.StartsWith('x'))
			{
				coefficient = 1m;
				divisionNeeded = true;
			}
			else if (side.Length == 2 && side.StartsWith('-') && side.Contains('x'))
			{
				side = "x";
				reverseSignNeeded = true;
			}
			else
			{
				decimal[] fraction;
				if (side.Contains('x'))
				{
					if (side.Contains('/') && side.Contains('*'))
					{
						decimal numerator;
						decimal denominator;
						string[] sideSplitted = side.Split(new char[] { '*', '/' }, StringSplitOptions.RemoveEmptyEntries);
						numerator = decimal.Parse(sideSplitted[0]);
						denominator = decimal.Parse(sideSplitted[1]);
						coefficient = numerator / denominator;
						divisionNeeded = true;
					}
					else if (side.Contains('/') && !side.Contains('*'))
					{
						coefficient = decimal.Parse(side.Split('/', StringSplitOptions.RemoveEmptyEntries)[1]);
						multiplicationNeeded = true;
					}
					else if (!side.Contains('/') && side.Contains('*'))
					{
						coefficient = coefficient = decimal.Parse(side.Split('*', StringSplitOptions.RemoveEmptyEntries)[0]);
						divisionNeeded = true;
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
			return new object[] { coefficient, divisionNeeded, multiplicationNeeded, reverseSignNeeded };
		}

	}
}
