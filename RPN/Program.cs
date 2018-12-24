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
                var rpnString = ParseToRpn(input);

                Console.WriteLine(GetResult(rpnString));
            }
        }

        static string ParseToRpn(string input)
        {
            var stack = new Stack<char>();
            var rpnString = string.Empty;

            foreach (char c in input.ToArray())
            {
                if (char.IsDigit(c))
                    rpnString += c;
                else
                {
                    if (c == ')')
                    {
                        while (stack.Count > 0 && stack.Peek() != '(')
                            rpnString += stack.Pop();

                        if (stack.Count > 0)
                            stack.Pop();

                        continue;
                    } 
                    else if (c == '(')
                    {
                        stack.Push(c);

                        continue;
                    }

                    if (stack.Count > 0 && stack.Peek() != '(')
                        if (GetPriority(c) <= GetPriority(stack.Peek()))
                            rpnString += stack.Pop();

                    stack.Push(c);
                }
            }

            if (stack.Count > 0)            
                rpnString += string.Concat(string.Join("", stack.ToArray()).Where(c => c != '('));            

            return rpnString;
        }

        static decimal GetResult(string rpnString)
        {
            if (rpnString == string.Empty)
                return 0;

            var stack = new Stack<decimal>();

            foreach (char c in rpnString.ToArray())
            {
                if (char.IsDigit(c))
                    stack.Push(decimal.Parse(c.ToString()));
                else
                    stack.Push(GetMathFunc(c)(stack.Pop(), stack.Pop()));
            }

            return stack.Pop();
        }

        static Func<decimal, decimal, decimal> GetMathFunc(char command)
        {
            switch (command)
            {
                case '+':
                    return (y, x) => x + y;
                case '-':
                    return (y, x) => x - y;
                case '/':
                    return (y, x) => x / y;
                case '*':
                    return (y, x) => x * y;
                case '^':
                    return (y, x) => (decimal)Math.Pow((double)x, (double)y);
                default:
                    return null;
            }
        }

        static int GetPriority(char symbol)
        {
            switch (symbol)
            {
                case '^':
                    return 4;
                case '*':
                case '/':
                    return 3;
                case '+':
                case '-':
                    return 2;
                case '(':
                case ')':
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
