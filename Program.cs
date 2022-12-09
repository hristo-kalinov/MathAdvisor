using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MathNet.Symbolics;
using System.Text;

namespace linear_equations
{
    internal class Program
    {
        public static string leftSideString = string.Empty;
        public static string rightSideString = string.Empty;
        static void Main(string[] args)
        {
            string inputExpr = "10*x + 5 + 2*x = x - 4";
            Console.WriteLine(inputExpr);
            if (inputExpr.Contains('=')) //check if the input is an equation(x+2=0) or a math problem(5+3*2)
            {
                Console.WriteLine(SolveEquation(inputExpr));
            }
            else
            {
                Console.WriteLine(SolveProblem(inputExpr)); 
            }
        }
        private static string SolveEquation(string expression)
        {
            SymbolicExpression leftSide;  //SymbolicExpression var of left side
            SymbolicExpression rightSide; //SymbolicExpression var of right side

            var splitEquation = expression.Split('='); //split left and right side of the equation

            leftSideString = splitEquation[0].Trim();
            rightSideString = splitEquation[1].Trim();

            leftSide = SymbolicExpression.Parse(leftSideString); //simplify left side
            rightSide = SymbolicExpression.Parse(rightSideString); //simplify right side

            string formattedText = string.Join(" ", leftSide.ToString().Split(' ').Reverse()) + " = " + string.Join(" ", rightSide.ToString().Split(' ').Reverse());
            Console.WriteLine("Simplify1: " + formattedText);

            leftSideString = "";
            rightSideString = "";

            ArrangeEquation(leftSide, true);
            ArrangeEquation(rightSide, false);

            leftSideString = leftSideString.Remove(0, 1);
            rightSideString = rightSideString.Remove(0, 1);
            if (leftSideString[0] == '+')
            {
                leftSideString = leftSideString.Remove(0, 1);
            }

            if (rightSideString[0] == '+')
            {
                rightSideString.Remove(0, 1);
            }
            leftSide = SymbolicExpression.Parse(leftSideString); //simplify left side
            rightSide = SymbolicExpression.Parse(rightSideString); //simplify right side

            string formattedText2 = string.Join(" ", leftSide.ToString().Split(' ').Reverse()) + " = " + string.Join(" ", rightSide.ToString().Split(' ').Reverse());
            Console.WriteLine("Simplify2: " + leftSideString + " = " + rightSideString);
            return $"Simplify2: {formattedText2}";
        }

        private static string SolveProblem(string expression)
        {
            string answer = SymbolicExpression.Parse(expression).ToString();
            return answer;
        }
        private static void ArrangeEquation(SymbolicExpression side, bool isLeft)
        {
            foreach (string inputNum in HandlePlusesAndMinuses(side.ToString()).Split(' '))
            {
                string formattedNum = inputNum;
                if (inputNum[0] != '+' && inputNum[0] != '-') //add a sign to the beginning
                {
                    formattedNum = "+" + inputNum;
                }
                switch (isLeft)
                {
                    case true:
                        switch (inputNum.Contains('x'))
                        {
                            case true: leftSideString += " " + formattedNum; break;
                            case false:
                                if (formattedNum[0] == '+') //change sign
                                {
                                    formattedNum = '-' + formattedNum.Remove(0, 1);
                                }
                                else
                                {
                                    formattedNum = '+' + formattedNum.Remove(0, 1);
                                }
                                rightSideString += " " + formattedNum; //add the the right side
                                break;
                        }
                        break;
                    case false:
                        switch (inputNum.Contains('x'))
                        {
                            case true:
                                if (formattedNum[0] == '+') //change sign
                                {
                                    formattedNum = '-' + formattedNum.Remove(0, 1);
                                }
                                else
                                {
                                    formattedNum = '+' + formattedNum.Remove(0, 1);
                                }
                                leftSideString += " " + formattedNum; 
                                break;
                            case false: rightSideString += " " + formattedNum; break;
                        }
                        break;
                }
            }
        }
        private static string HandlePlusesAndMinuses(string input)//search for pluses and minuses and remove the spaces after them so they have a sign such as +2, -2 instead of + 2
        {
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