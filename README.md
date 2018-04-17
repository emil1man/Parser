# Parser
Return result for f(x)
This .dll allows you to get the result of f(x), where f is the function inputed during the execution of the program, and x is the parameter of the function.
Entry rules:
1. Sin, Cos, Tan should be inserted together with the brackets: 'sin( x )', etc;
2. The program allows you to work with pi and exponent ('pi' and 'e');
3. Any numbers, signs and parentheses must be separated by a space, unless otherwise stated.
Example of use:

using System;
using parser;

namespace brain
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Console.Write("input f: ");
                    Parser p = new Parser(Console.ReadLine());
                    Console.Write("input x: ");
                    Console.WriteLine("f(x) = " + p.calcFun(double.Parse(Console.ReadLine())));
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
        }
    }
}

Results:
input f: 1
input x: 2
f(x) = 1
input f: sin( x )
input x: 90
f(x) = 1
input f: 2 * cos( 7 * pi )
input x: 1
f(x) = 1,85448343033348
input f: 2 * cos( x * pi ) - e
input x: 2
f(x) = -0,73029559531401
input f: e
input x: 1
f(x) = 2,71828182845905
input f: 2 * cos( x * pi ) - e ^ 3
input x: 0
f(x) = -18,0855369231878
input f:
