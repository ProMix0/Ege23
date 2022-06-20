using System;
using System.Collections.Generic;
using System.Linq;

namespace Ege23
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine((int)Variants.Count(1, 410, new()
            {
                a => a + 1,
                a => a switch
                {
                    _ when (a * 10 + 1) % 3 == 0 => a * 10 + 1,
                    _ => a
                },
                a => a * 5
            }).Include(40));*/
            Console.WriteLine(Recursion.Variants(1));
        }
    }
}
