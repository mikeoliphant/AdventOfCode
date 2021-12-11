﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    public static class ParseHelpers
    {
        public static string[] SplitLines(this string input)
        {
            return input.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        }

        public static string[] SplitParagraphs(this string input)
        {
            return input.TrimEnd().Split(new string[] { "\r\n\r\n", "\r\r", "\n\n" }, StringSplitOptions.None);
        }

        public static IEnumerable<int> ToInts(this string input, char delimeter)
        {
            return from intStr in input.Split(delimeter) select int.Parse(intStr);
        }

        public static IEnumerable<int> ToInts(this string input, string delimeter)
        {
            return from intStr in input.Split(delimeter) select int.Parse(intStr);
        }
    }
}
