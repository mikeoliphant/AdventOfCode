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
            LinkedListNode<int>[] nodeArray;
            LinkedList<int> list;
            LinkedListNode<int> currentNode;

            int minVal;
            int maxVal;           

            public CircleList(List<int> startValues)
            {
                list = new LinkedList<int>(startValues);

                currentNode = list.First;

                minVal = list.Min();
                maxVal = list.Max();

                nodeArray = new LinkedListNode<int>[list.Count];

                int pos = 0;

                LinkedListNode<int> node = list.First;

                do
                {
                    nodeArray[node.Value - 1] = node;

                    node = node.Next;
                }
                while (node != null);
            }

            public int Count { get { return list.Count; } }

            public List<int> RemoveClockwise(int numToRemove)
            {
                List<int> toReturn = new List<int>();

                for (int i = 0; i < numToRemove; i++)
                {
                    if (currentNode.Next == null)
                    {
                        toReturn.Add(list.First.Value);

                        list.RemoveFirst();
                    }
                    else
                    {
                        toReturn.Add(currentNode.Next.Value);
                        list.Remove(currentNode.Next);
                    }
                }

                return toReturn;
            }

            LinkedListNode<int> lastDest = null;

            public void InsertAtDestination(List<int> toInsert)
            {
                int value = currentNode.Value - 1;

                if (value < minVal)
                {
                    value = maxVal;
                }

                while (toInsert.Contains(value))
                {
                    value--;

                    if (value < minVal)
                    {
                        value = maxVal;
                    }
                }

                LinkedListNode<int> destinationNode = nodeArray[value - 1];

                lastDest = destinationNode;

                for (int i = toInsert.Count - 1; i >= 0; i--)
                    list.AddAfter(destinationNode, nodeArray[toInsert[i] - 1]);
            }

            public void SetCurrentValue(int val)
            {
                currentNode = nodeArray[val - 1];
            }

            public void IncrementCurrent()
            {
                if (currentNode == list.Last)
                    currentNode = list.First;
                else
                    currentNode = currentNode.Next;
            }

            public override string ToString()
            {
                return String.Join(',', from num in list select (num == currentNode.Value) ? "(" + num + ")" : num.ToString());
            }
        }

        public long Compute()
        {
            //CircleList circle = new CircleList(new List<int> { 3, 8, 9, 1, 2, 5, 4, 6, 7 });
            CircleList circle = new CircleList(new List<int> { 6, 2, 4, 3, 9, 7, 1, 5, 8 });


            for (int move = 0; move < 100; move++)
            {
                List<int> removed = circle.RemoveClockwise(3);
                circle.InsertAtDestination(removed);
                circle.IncrementCurrent();
            }

            circle.SetCurrentValue(1);
            List<int> rest = circle.RemoveClockwise(circle.Count - 1);

            long result = long.Parse(String.Join("", rest));

            return result;
        }

        public long Compute2()
        {
            //CircleList circle = new CircleList(new List<int> { 3, 8, 9, 1, 2, 5, 4, 6, 7 });
            //CircleList circle = new CircleList(new List<int> { 6, 2, 4, 3, 9, 7, 1, 5, 8 });

            //List<int> nums = new List<int> { 3, 8, 9, 1, 2, 5, 4, 6, 7 };
            List<int> nums = new List<int> { 6, 2, 4, 3, 9, 7, 1, 5, 8 };

            for (int i = 10; i <= 1000000; i++)
                nums.Add(i);

            CircleList circle = new CircleList(nums);

            for (int move = 0; move < 10000000; move++)
            {
                List<int> removed = circle.RemoveClockwise(3);
                circle.InsertAtDestination(removed);
                circle.IncrementCurrent();
            }

            circle.SetCurrentValue(1);
            List<int> starCups = circle.RemoveClockwise(2);

            long result = (long)starCups[0] * (long)starCups[1];

            return result;
        }
    }
}
