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
            private List<Rule> rules;
            private Dictionary<int, int> table;

            private int result;
            private bool haveResult=false;

            internal VariantsResult(int start, int stop, List<Rule> rules)
            {
                this.start = start;
                this.stop = stop;
                this.rules = rules;
                table = new();
            }

            #region extensions

            private List<int> include = new(), exclude = new();

            public VariantsResult Include(List<int> include)
            {
                this.include.AddRange(include);

                return this;
            }
            public VariantsResult Include(int include)
            {
                this.include.Add(include);

                return this;
            }

            public VariantsResult Exclude(List<int> exclude)
            {
                this.exclude.AddRange(exclude);

                return this;
            }
            public VariantsResult Exclude(int exclude)
            {
                this.exclude.Add(exclude);

                return this;
            }

            #endregion


            public static explicit operator int(VariantsResult param) => param.Result();

            //public static implicit operator int(VariantsResult param)=>param.ToInt();

            private int Result()
            {
                if (!haveResult)
                {
                    for (int i = start; i <= stop; i++)
                    {
                        table.Add(i, 0);
                    }
                    table[start] = 1;

                    for (int i = start; i <= stop; i++)
                    {
                        foreach (var rule in rules)
                        {
                            int number = rule(i);
                            if (table.ContainsKey(number))
                                table[number] += table[i];
                        }
                    }
                }

                result = table[stop];
                haveResult = true;

                table = null;
                rules = null;

                return result;
            }

            public override string ToString()
            {
                return Result().ToString();
            }
        }

        public delegate int Rule(int input);
    }
}
