using Spades.Testing;
using System;

namespace Spades
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Starting...");
            Tests tests = new Tests();
            tests.testAll();
            Console.ReadLine();
        }
    }
}
