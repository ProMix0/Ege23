using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ege23
{
    public class Variants
    {
        private readonly int start;
        private readonly int stop;
        private List<Rule> rules;
        private Dictionary<int, int> table;

        private int result;
        private bool haveResult = false;

        internal Variants(int start, int stop, List<Rule> rules)
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

        public Variants Include(params int[] include)
        {
            return Include((IEnumerable<int>)include);
        }
        public Variants Include(IEnumerable<int> include)
        {
            foreach (var i in include)
                Include(i);

            return this;
        }
        public Variants Include(int include)
        {
            if (!table.ContainsKey(include))
                throw new ArgumentOutOfRangeException(nameof(include));

            this.include.Add(include);

            return this;
        }

        public Variants Exclude(params int[] exclude)
        {
            return Exclude((IEnumerable<int>)exclude);
        }
        public Variants Exclude(IEnumerable<int> exclude)
        {
            foreach (var i in exclude)
                Exclude(i);

            return this;
        }
        public Variants Exclude(int exclude)
        {
            if (!table.ContainsKey(exclude))
                throw new ArgumentOutOfRangeException(nameof(exclude));

            this.exclude.Add(exclude);

            return this;
        }

        #endregion


        //public static explicit operator int(VariantsResult param) => param.Result();

        public static implicit operator int(Variants param) => param.Result();

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
                if (!table.ContainsKey(i)) continue;
                foreach (var rule in rules)
                {
                    int number = rule(i);
                    if (table.ContainsKey(number) && (number <= start || number <= stop))
                        table[number] += table[i];
                }
            }
        }

        public override string ToString()
        {
            return Result().ToString();
        }

        public delegate int Rule(int input);

        public static Variants Count(int start, int stop, List<Rule> rules)
        {
            return new(start, stop, rules);
        }

    }
}
