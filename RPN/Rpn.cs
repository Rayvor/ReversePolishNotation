using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN
{
    public class Rpn
    {
        private readonly string _input;

        public Rpn(string input)
        {
            _input = input;
        }

        public decimal Result
        {
            get
            {
                var inputArray = SplitByNums(_input);
                var rpn = ParseToRpn(inputArray);

                return GetResult(rpn);
            }
        }

        private List<string> ParseToRpn(string[] input)
        {
            var stack = new Stack<string>();
            var rpn = new List<string>();

            foreach (string s in input)
            {
                if (IsDigit(s))
                    rpn.Add(s);
                else
                {
                    if (s == ")")
                    {
                        while (stack.Count > 0 && stack.Peek() != "(")
                            rpn.Add(stack.Pop());

                        if (stack.Count > 0)
                            stack.Pop();

                        continue;
                    }
                    else if (s == "(")
                    {
                        stack.Push(s);

                        continue;
                    }

                    if (stack.Count > 0 && stack.Peek() != "(")
                        if (GetPriority(s) <= GetPriority(stack.Peek()))
                            rpn.Add(stack.Pop());

                    stack.Push(s);
                }
            }

            if (stack.Count > 0)
                rpn.AddRange(stack.Where(s => s != "("));

            return rpn;
        }

        private decimal GetResult(List<string> rpn)
        {
            if (rpn.Count == 0)
                return 0;

            var stack = new Stack<decimal>();

            foreach (string s in rpn)
            {
                if (IsDigit(s))
                    stack.Push(decimal.Parse(s));
                else
                    stack.Push(GetMathFunc(s)(stack.Pop(), stack.Pop()));
            }

            return stack.Pop();
        }

        private Func<decimal, decimal, decimal> GetMathFunc(string command)
        {
            switch (command)
            {
                case "+":
                    return (y, x) => x + y;
                case "-":
                    return (y, x) => x - y;
                case "/":
                    return (y, x) => x / y;
                case "*":
                    return (y, x) => x * y;
                case "^":
                    return (y, x) => (decimal)Math.Pow((double)x, (double)y);
                default:
                    return null;
            }
        }

        private int GetPriority(string symbol)
        {
            switch (symbol)
            {
                case "^":
                    return 4;
                case "*":
                case "/":
                    return 3;
                case "+":
                case "-":
                    return 2;
                default:
                    return 0;
            }
        }

        private bool IsDigit(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }

        private string[] SplitByNums(string str)
        {
            var indexesForSubstring = new List<int>();

            bool isFirstDigit = true;

            for (int i = 0; i < str.Length; i++)
            {
                if (isFirstDigit && char.IsDigit(str[i]))
                {
                    indexesForSubstring.Add(i);
                    isFirstDigit = false;
                }
                else if (!char.IsDigit(str[i]))
                {
                    indexesForSubstring.Add(i);
                    isFirstDigit = true;
                }
            }

            indexesForSubstring.Add(str.Length);

            string[] result = new string[indexesForSubstring.Count - 1];

            for (int i = 0; i < result.Length; i++)
            {
                int length = indexesForSubstring[i + 1] - indexesForSubstring[i];
                result[i] = str.Substring(indexesForSubstring[i], length);
            }

            return result;
        }
    }
}
