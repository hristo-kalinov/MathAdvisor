using ApiTest.MathAdvisorLogic;
using MathAdvisor.PreAlgebra;
using MathNet.Symbolics;

namespace MathAdvisor.Algebra
{
    /// <summary>
    /// Class for solving quadratic equations
    /// </summary>
    public class QuadraticEquation
    {
        /// <summary>
        /// Main method for solving the equation
        /// </summary>
        /// <param name="inputEquation">Initial input equation</param>
        /// <returns>Solved equation for x</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static void SolveQuadratic(string leftSide, string rightSide)
        {
            string[] equationArray = CommonMath.NormalizeEquation(leftSide, rightSide, true);
            string rawExpression = string.Concat(equationArray[0], " = ", equationArray[1]);
            leftSide = StringHandling.ReverseEquation(SymbolicExpression.Parse(equationArray[0]).ToString()); //Parse left side; no need for right side implementation since it's always 0
            leftSide = StringHandling.HandlePlusesAndMinuses(leftSide);
            DiscriminantFormula(leftSide);
        }

        public static void DiscriminantFormula(string leftSide)
        {
            string[] parameters = leftSide.Split(' ');
            string a = "0";
            string b = "0";
            string c = "0";

            if (parameters[0][1] != 'x')
            {
                a = parameters[0].Split('*')[0];
            }
            else
            {
                a = "1";
            }
            if (parameters.Length == 3)
            {
                if (parameters[1][1] != 'x')
                {
                    b = parameters[1].Split('*')[0];
                }
                else
                {
                    b = "1";
                }
                c = parameters[2];
                QuadraticFormula(a, b, c);
            }
            else if(parameters.Length == 2)
            {
                if (parameters[1].Contains('x'))
                {
                    if (parameters[1][1] != 'x')
                    {
                        b = parameters[1].Split('x')[1];
                    }
                    else
                    {
                        b = "1";
                    }
                }
                else
                {
                    c = parameters[1];
                }
                QuadraticFormula(a, b, c);
            }

            else
            {
                Solver.solution += "x = 0";
            }

        }

        private static void QuadraticFormula(string a, string b, string c)
        {
            a = AddParenthesesOnNegatives(a);
            b = AddParenthesesOnNegatives(b);
            c = AddParenthesesOnNegatives(c);
            string discriminant = SymbolicExpression.Parse($"{b}^2 - 4*{a}*{c}").ToLaTeX();
            Solver.solution += $"Solve the Discriminant using the formula b^2 - 4ac: {b}^2 - 4*{a}*{c} = {discriminant}\n";
            if (int.Parse(discriminant) < 0)
            {
                Solver.solution += $"{discriminant} is less than 0 therefore the equation doesn't have an answer";
                return;
            }
            var dInfix = Infix.ParseOrThrow($"sqrt({discriminant})");
            string realValue = Evaluate.Evaluate(null, dInfix).RealValue.ToString();
            if (!realValue.Contains("."))
            {
                discriminant = realValue;
            }
            else
            {
                discriminant = $"sqrt({discriminant})";
            }

            string firstAnswer = SymbolicExpression.Parse($"(-{b} + {discriminant})/(2*{a})\n").ToString();
            string secondAnswer = SymbolicExpression.Parse($"(-{b} - {discriminant})/(2*{a})\n").ToString();
            Solver.solution += $"Solve for x1:({SymbolicExpression.Parse($"-{b}")} + {discriminant})/(2*{a})\n";
            Solver.solution += $"Solve for x2:({SymbolicExpression.Parse($"-{b}")} - {discriminant})/(2*{a})\n";
            Solver.answers.Add(Evaluate.Evaluate(null, Infix.ParseOrThrow(firstAnswer)).RealValue.ToString());
            Solver.answers.Add(Evaluate.Evaluate(null, Infix.ParseOrThrow(secondAnswer)).RealValue.ToString());

        }
        private static string AddParenthesesOnNegatives(string number)
        {
            if (number[0] == '-')
                return $"({number})";
            else
                return number;
        }
    }
}
