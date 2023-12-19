using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2023
{
    internal class Day4 : Day
    {
        public override long Compute()
        {
            var cards = File.ReadLines(DataFile);

            long sum = 0;

            foreach (string card in cards)
            {
                string[] numbers = card.Split(':')[1].Split('|');

                var winners = numbers[0].Trim().Replace("  ", " ").ToInts(' ').ToArray();
                var have = numbers[1].Trim().Replace("  ", " ").ToInts(' ').ToArray();

                long value = 0;

                foreach (int winner in winners)
                {
                    if (have.Contains(winner))
                    {
                        if (value == 0)
                            value = 1;
                        else
                            value *= 2;
                    }
                }

                sum += value;
            }

            return sum;
        }

        public override long Compute2()
        {
            var cards = File.ReadLines(DataFile);

            Dictionary<int, long> cardValue = new();

            int cardIndex = 1;

            foreach (string card in cards)
            {
                string[] numbers = card.Split(':')[1].Split('|');

                var winners = numbers[0].Trim().Replace("  ", " ").ToInts(' ').ToArray();
                var have = numbers[1].Trim().Replace("  ", " ").ToInts(' ').ToArray();

                int value = 0;

                foreach (int winner in winners)
                {
                    if (have.Contains(winner))
                    {
                        value++;
                    }
                }

                cardValue[cardIndex] = value;

                cardIndex++;
            }

            Dictionary<int, long> cardNumbers = new();

            foreach (int index in cardValue.Keys)
            {
                cardNumbers[index] = 1;
            }

            for (int index = 1; index < (cardNumbers.Count + 1); index++)
            {
                for (int index2 = index + 1; index2 < index + cardValue[index] + 1; index2++)
                {
                    if (index2 == (cardNumbers.Count + 1))
                        break;

                    cardNumbers[index2] += cardNumbers[index];
                }
            }

            long sum = 0;

            foreach (long value in cardNumbers.Values)
            {
                sum += value;
            }

            return 0;
        }
    }
}
