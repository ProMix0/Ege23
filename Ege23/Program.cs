using System;
using System.Collections.Generic;
using System.Linq;

namespace Ege23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine((int)Variants.Count(1, 20, new()
            {
                a => a + 1,
                a => a * 3
            }).Exclude(10));
        }
    }
}
