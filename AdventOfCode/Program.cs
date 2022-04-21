global using System;
global using System.Collections;
global using System.Collections.Generic;
global using System.Drawing;
global using System.IO;
global using System.Linq;
global using System.Numerics;
global using System.Text;
global using System.Text.RegularExpressions;
global using System.Threading;

namespace AdventOfCode
{
    class Program
    {
        static Thread runThread = null;

        static void Main(string[] args)
        {
            long result = new AdventOfCode._2018.Day18().Compute2();
        }
    }
}
