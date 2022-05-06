using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode
{
    internal static class MathHelper
    {
        public static int PosMod(int value, int mod)
        {
            int r = value % mod;

            return r < 0 ? r + mod : r;
        }
    }
}
