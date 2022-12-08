using Fractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;
using System.Text;

namespace linear_equations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SymbolicExpression leftSide;  //SymbolicExpression var of left side
            SymbolicExpression rightSide;

            string leftSideString; //string var of left side
            string rightSideString;

            string input = "2*x + 5*x + 3 + 9 = 10 + 1";

            Console.WriteLine(input);

            if (input.Contains("=")) //check if the input is an equation(x+2=0) or a math problem(5+3*2)
            {
                var splitEquation = input.Split('='); //split left and right side of the equation

                leftSideString = splitEquation[0];
                rightSideString = splitEquation[1];

                leftSide = Expr.Parse(leftSideString); //simplify both sides
                rightSide = Expr.Parse(rightSideString);

                string formattedText = FormatExp("Simplify: " + leftSide.ToString() + " = " + rightSide.ToString());
                Console.WriteLine(formattedText);

                leftSideString = "";
                rightSideString = "";
                foreach (string num in HandlePlusesAndMinuses(leftSide.ToString()).Split(' ')) //transfer the variables to the left and the numbers to the right
                {
                    if (num[0] != '+' || num[0] != '-') //add a sign to the beginning
                    {
                        string num1 = "+" + num;
                    }

                    if (num.Contains("x"))
                    {
                        leftSideString += " +" + num;
                    }
                    else
                    {
                        //if(u)
                    }
                }

                foreach (string num in HandlePlusesAndMinuses(rightSide.ToString()).Split(' '))
                {
                    if (num[0] != '+' || num[0] != '-')
                    {
                        leftSideString = "+" + num;
                    }
                    if (num.Contains("x"))
                    {
                        leftSideString += " -" + num;
                    }
                    else
                    {
                        rightSideString += " +" + num;
                    }
                }

                leftSideString = leftSideString.Remove(0, 1);
                rightSideString = rightSideString.Remove(0, 1);

            }
            else
            {
                leftSide = Expr.Parse(input);
            }
        }

        static string FormatExp(SymbolicExpression exp) //make the expression in the format ax^2 + bx + c = 0
        {
            return (string.Join(" ", exp.ToString().Split(' ').Reverse()));
        }

        public static string HandlePlusesAndMinuses(string input)//search for pluses and minuses and remove the spaces after them so they have a sign such as +2, -2 instead of + 2
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


