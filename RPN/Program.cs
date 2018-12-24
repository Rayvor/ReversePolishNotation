using System;
using System.Collections.Generic;
using System.Linq;

namespace RPN
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var input = Console.ReadLine();
                var rpn = ParseToRpn(input.Split(' '));

                decimal result = GetResult(rpn);

                Console.WriteLine(result);
            }
        }

        static List<string> ParseToRpn(string[] input)
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

        static decimal GetResult(List<string> rpn)
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

        static Func<decimal, decimal, decimal> GetMathFunc(string command)
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

        static int GetPriority(string symbol)
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
                case "(":
                case ")":
                    return 1;
                default:
                    return 0;
            }
        }

        static bool IsDigit(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }

            return true;
        }
    }
}
