using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    public class Day10
    {
        Dictionary<char, char> parens = new Dictionary<char, char> { { ')', '(' }, { '>', '<' }, { ']', '[' }, { '}', '{' }, };
        Dictionary<char, char> parensInverse = new Dictionary<char, char> { { '(', ')' }, { '<', '>' }, { '[', ']' }, { '{', '}' }, };
        string[] lines;

        void ReadInput()
        {
            lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day10.txt").ToArray();
        }

        Stack<char> parenStack = new Stack<char>();

        bool IsCorrupt(string line, out int pos)
        {
            pos = 0;

            parenStack = new Stack<char>();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (parens.ContainsKey(c))
                {
                    if (parenStack.Pop() != parens[c])
                    {
                        pos = i;

                        return true;
                    }
                }
                else
                {
                    parenStack.Push(c);
                }
            }

            return false;
        }

        public long Compute()
        {
            ReadInput();

            int totScore = 0;

            foreach (string line in lines)
            {
                int corruptPos;

                if (IsCorrupt(line, out corruptPos))
                {
                    int score = 0;

                    if (corruptPos != -1)
                    {
                        switch (line[corruptPos])
                        {
                            case ')':
                                score = 3;
                                break;

                            case ']':
                                score = 57;
                                break;

                            case '}':
                                score = 1197;
                                break;

                            case '>':
                                score = 25137;
                                break;
                        }
                    }

                    totScore += score;
                }
            }

            return totScore;
        }

        public long Compute2()
        {
            ReadInput();

            List<long> scores = new List<long>();

            foreach (string line in lines)
            {
                int corruptPos;

                if (!IsCorrupt(line, out corruptPos))
                {
                    string termination = "";

                    while (parenStack.Count > 0)
                    {
                        termination += parensInverse[parenStack.Pop()];
                    }

                    long lineScore = 0;

                    foreach (char c in termination)
                    {
                        int score = 0;

                        switch (c)
                        {
                            case ')':
                                score = 1;
                                break;

                            case ']':
                                score = 2;
                                break;

                            case '}':
                                score = 3;
                                break;

                            case '>':
                                score = 4;
                                break;
                        }

                        lineScore = (lineScore * 5) + score;
                    }

                    scores.Add(lineScore);
                }
            }

            scores.Sort();

            return scores[scores.Count / 2];
        }
    }
}
