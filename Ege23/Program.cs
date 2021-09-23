using System;
using System.Collections.Generic;

namespace Ege23
{
    class Program
    {
        static void Main(string[] args)
        {            
            Console.WriteLine(VariantsCount(1,10, new()
            {
                a => a + 1,
                a => a * 3
            }));
        }

        public static VariantsResult VariantsCount(int start, int stop, List<Rule> rules)
        {
            return new(start, stop, rules);
        }

        public class VariantsResult
        {
            private readonly int start;
            private readonly int stop;
            private readonly List<Rule> rules;
            private readonly Dictionary<int, int> table;

            internal VariantsResult(int start, int stop, List<Rule> rules)
            {
                this.start = start;
                this.stop = stop;
                this.rules = rules;
                table = new();
            }


            public static explicit operator int(VariantsResult param)
            {
                Dictionary<int, int> table = new();
                for (int i = param.start; i <= param.stop; i++)
                {
                    table.Add(i, 0);
                }
                table[param.start] = 1;

                for (int i = param.start; i <= param.stop; i++)
                {
                    foreach (var rule in param.rules)
                    {
                        int number = rule(i);
                        if (table.ContainsKey(number))
                            table[number] += table[i];
                    }
                }

                return param.table[param.stop];
            }
        }

        public delegate int Rule(int input);
    }
}
