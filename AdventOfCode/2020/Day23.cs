using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    internal class Day23
    {
        internal class CircleList
        {
            public List<int> list;
            int currentValue = 0;

            public CircleList(List<int> startValues)
            {
                list = new List<int>(startValues);

                currentValue = list[0];
            }

            public List<int> RemoveClockwise(int numToRemove)
            {
                List<int> toReturn = new List<int>();

                int currentPosition = list.IndexOf(currentValue);

                for (int i = 0; i < numToRemove; i++)
                {
                    int removePos = (currentPosition + 1) % list.Count;

                    toReturn.Add(list[removePos]);
                    list.RemoveAt(removePos);
                }

                return toReturn;
            }

            public void InsertAtDestination(List<int> toInsert)
            {
                int value = currentValue - 1;

                int minValue = list.Min();                

                while (!list.Contains(value))
                {
                    if (value < minValue)
                    {
                        value = list.Max();
                        break;
                    }

                    value--;
                }

                int destination = list.IndexOf(value);

                list.InsertRange(destination + 1, toInsert);
            }

            public void IncrementCurrent()
            {
                currentValue = list[(list.IndexOf(currentValue) + 1) % list.Count];
            }

            public override string ToString()
            {
                return String.Join(',', list);
            }
        }

        public long Compute()
        {
            CircleList circle = new CircleList(new List<int> { 3, 8, 9, 1, 2, 5, 4, 6, 7 });

            for (int move = 0; move < 10; move++)
            {
                List<int> removed = circle.RemoveClockwise(3);
                circle.InsertAtDestination(removed);
                circle.IncrementCurrent();
            }

            return 0;
        }
    }
}
