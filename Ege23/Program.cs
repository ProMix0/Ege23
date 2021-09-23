using System;
using System.Collections.Generic;
using System.Linq;

namespace Ege23
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(VariantsCount(1, 20, new()
            {
                a => a + 1,
                a => a * 3
            })/*.Include(10)*/);
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
            private bool haveResult = false;

            internal VariantsResult(int start, int stop, List<Rule> rules)
            {
                this.start = start;
                this.stop = stop;
                this.rules = rules;

                include = new() { start, stop };
                exclude = new();

                table = new();
                for (int i = start; i <= stop; i++)
                {
                    table.Add(i, 0);
                }
                table[start] = 1;
            }

            #region extensions

            private List<int> include, exclude;

            public VariantsResult Include(List<int> include)
            {
                foreach (var i in include)
                    Include(i);

                return this;
            }
            public VariantsResult Include(int include)
            {
                if (!table.ContainsKey(include))
                    throw new ArgumentOutOfRangeException(nameof(include));

                this.include.Add(include);

                return this;
            }

            public VariantsResult Exclude(List<int> exclude)
            {
                foreach (var i in exclude)
                    Exclude(i);

                return this;
            }
            public VariantsResult Exclude(int exclude)
            {
                if (!table.ContainsKey(exclude))
                    throw new ArgumentOutOfRangeException(nameof(exclude));

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
                    include.Sort();
                    foreach (var i in exclude.Distinct())
                        table.Remove(i);

                    int? previous = null;
                    foreach (var i in include)
                    {
                        if (previous != null)
                            SubResult(previous.Value, i);
                        previous = i;
                    }
                }

                result = table[stop];
                haveResult = true;

                table = null;
                rules = null;
                include = null;
                exclude = null;

                return result;
            }

            private void SubResult(int start, int stop)
            {
                if (start == stop) return;

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

            /*public override string ToString()
            {
                return Result().ToString();
            }*/
        }

        public delegate int Rule(int input);
    }
}
