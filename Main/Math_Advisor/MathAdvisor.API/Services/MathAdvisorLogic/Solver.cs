namespace Math_Advisor.API.Services.MathAdvisorLogic
{
	using Math_Advisor.API.Services.MathAdvisorLogic.Algebra;
	using Math_Advisor.API.Services.MathAdvisorLogic.PreAlgebra;
	using MathNet.Symbolics;
	using System;
	using System.Collections.Generic;
	public static class Solver
	{
		//append part of the solution to this string
		public static string solution = string.Empty;
		public static List<string> answers = new List<string>();
		public static string Solve(string inputExpression)
		{
			solution = string.Empty;
			answers.Clear();
			if (inputExpression.Contains('='))
			{

				solution += $"{inputExpression}\n";
				string formattedExpression = StringHandling.SymbolHandling(inputExpression);

				string leftSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[0]!.Trim();
				string rightSide = formattedExpression.Split('=', StringSplitOptions.TrimEntries)[1]!.Trim();
				string prevLeft = leftSide;
				string prevRight = rightSide;
				leftSide = CommonMath.CheckForParentheses(leftSide);
				rightSide = CommonMath.CheckForParentheses(rightSide);

				if (StringHandling.EquationsAreDifferent(prevLeft, prevRight, leftSide, rightSide))
					solution += $"Expand: {leftSide} = {rightSide}\n";

				prevLeft = leftSide;
				prevRight = rightSide;

				string[] outputExpression = CommonMath.NormalizeEquation(leftSide, rightSide, false);
				leftSide = outputExpression[0];
				rightSide = outputExpression[1];

				if (StringHandling.EquationsAreDifferent(prevLeft, prevRight, leftSide, rightSide))
					solution += $"Transfer variables: {leftSide} = {rightSide}\n";

				prevLeft = leftSide;
				prevRight = rightSide;

				leftSide = SymbolicExpression.Parse(leftSide).ToString();
				rightSide = SymbolicExpression.Parse(rightSide).ToString();

				if (StringHandling.EquationsAreDifferent(prevLeft, prevRight, leftSide, rightSide))
					solution += $"Simplify: {leftSide} = {rightSide}\n";

				prevLeft = leftSide;
				prevRight = rightSide;

				string simplified = string.Concat(leftSide, " = ", rightSide);
				if (inputExpression.Contains('x'))
				{
					if (!leftSide.Contains('^'))
					{
						LinearEquation.SolveLinear(leftSide, rightSide);
					}
					else
					{
						QuadraticEquation.SolveQuadratic(leftSide, rightSide);
					}
				}
				else
				{
					solution += "Check if both sides are equal:";
					if (leftSide == rightSide)
					{
						solution += "true";
					}
					else
					{
						solution += "false";
					}
				}
			}

			else
			{
				solution += inputExpression + "\n";
				solution += $"Simplify: {SymbolicExpression.Parse(inputExpression)}\n";
			}
			return solution;

		}
	}
}
