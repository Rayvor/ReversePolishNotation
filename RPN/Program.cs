using System;

namespace RPN
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                string input = Console.ReadLine();

                var rpn = new Rpn(input);

                Console.WriteLine(rpn.Result);
            }
        }        
    }
}
