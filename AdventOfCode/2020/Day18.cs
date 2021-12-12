using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day18
    {
        string[] expressions;

        void ReadInput()
        {
            expressions = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day18.txt").ToArray();
        }

        string AddAddPrecedence(string expression)
        {
            string[] adds = (from addExpr in expression.Split(" * ") select "(" + addExpr + ")").ToArray();

            return String.Join(" * ", adds);
        }

        long EvaluateExpression(string expression, bool doAddPrecedence)
        {
            int startPos = expression.IndexOf('(');

            if (startPos != -1)
            {
                int numParens = 0;

                for (int i = startPos; i <= expression.Length; i++)
                {
                    if (expression[i] == '(')
                    {
                        numParens++;
                    }
                    else if (expression[i] == ')')
                    {
                        numParens--;

                        if (numParens == 0)
                        {
                            string subExpression = expression.Substring(startPos, (i - startPos) + 1);

                            expression = expression.Replace(subExpression, EvaluateExpression(subExpression.Substring(1, subExpression.Length - 2), doAddPrecedence).ToString());

                            return EvaluateExpression(expression, doAddPrecedence);
                        }
                    }
                }
            }

            if (doAddPrecedence)
            {
                return EvaluateExpression(AddAddPrecedence(expression), doAddPrecedence: false);
            }

            string[] expr = expression.Split(' ');

            long value = long.Parse(expr[0]);

            for (int i = 1; i < expr.Length; i += 2)
            {
                if (expr[i][0] == '+')
                {
                    value += long.Parse(expr[i + 1]);
                }
                else if (expr[i][0] == '*')
                {
                    value *= long.Parse(expr[i + 1]);
                }
            }

            return value;
        }

        public long Compute()
        {
            ReadInput();

            long sum = 0;

            foreach (string expression in expressions)
            {
                sum += EvaluateExpression(expression, doAddPrecedence: false);
            }

            return sum;
        }

        public long Compute2()
        {
            ReadInput();

            long sum = 0;

            foreach (string expression in expressions)
            {
                sum += EvaluateExpression(expression, doAddPrecedence: true);
            }

            return sum;
        }
    }
}
