﻿namespace Math_Advisor.API.Services.MathAdvisorLogic.PreAlgebra
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	public static class StringHandling
    {
        // Add plus or minus in front of the first number so the sign becomes known for the <currentSideSigns> array

        internal static string AddSignInBeginning(string side)
        {
            if (!side.StartsWith('-'))
            {
                return string.Concat("+", side);
            }
            return side;
        }

        // Add all signs from the current side to the <currentSideSigns> array
        internal static List<char> AddSignsInArray(string side)
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

        // Method for removal of unnecessary numbers and signs
        internal static List<string> RemoveRemainingNumbersAndSigns(List<string> currentSideNumbers, List<string> currentSideNumbersToBeRemoved)
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

        //3 + x + x^2 becomes x^2 + x + 3
        public static string ReverseEquation(string input)
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

        public static bool EquationsAreDifferent(string prevLeft, string prevRight, string left, string right)
        {
            if(prevLeft != left || prevRight != right)
            {
                return true;
            }

            return false;
        }

        public static string AddMultiplicationSign(string input)
        {
            StringBuilder sb = new StringBuilder(input);
            if (input.Contains("x") && input[0] != 'x')
            {
                sb.Insert(input.IndexOf('x'), "*");
            }
            return sb.ToString();
        }
    }
}
