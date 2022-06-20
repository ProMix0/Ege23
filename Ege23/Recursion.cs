using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ege23
{
    internal static class Recursion
    {
        public static int Variants(int number)
        {
            if (number > 410) return 0;
            if (number == 410) return 1;

            int result = Variants(number + 1) + Variants(number * 5);
            if ((number * 10 + 1) % 3 == 0)
                result += Variants(number * 10 + 1);

            return result;
        }
    }
}
