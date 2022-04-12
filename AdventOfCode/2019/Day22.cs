using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;


namespace AdventOfCode._2019
{
    internal class Day22
    {
        public long Compute()
        {
            int numCards = 10007;

            int[] cards = new int[numCards];
            int[] cards2 = new int[numCards];

            for (int i = 0; i < numCards; i++)
                cards[i] = i;

            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day22.txt").ToArray();

            foreach (string command in commands)
            {
                string[] words = command.Split(' ');

                if (words[0] == "deal")
                {
                    if (words[1] == "into")
                    {
                        for (int i = 0; i < numCards; i++)
                        {
                            cards2[i] = cards[numCards - 1 - i];
                        }
                    }
                    else
                    {
                        int increment = int.Parse(words[3]);
                        int pos = 0;

                        for (int i = 0; i < numCards; i++)
                        {
                            cards2[pos] = cards[i];

                            pos = (pos + increment) % numCards;
                        }
                    }
                }
                else if (words[0] == "cut")
                {
                    int numToCut = int.Parse(words[1]);

                    if (numToCut > 0)
                    {
                        for (int i = 0; i < numCards; i++)
                        {
                            cards2[i] = cards[(i + numToCut) % numCards];
                        }
                    }
                    else
                    {
                        numToCut = -numToCut;

                        for (int i = 0; i < numCards; i++)
                        {
                            cards2[(i + numToCut) % numCards] = cards[i];
                        }
                    }
                }

                int[] tmp = cards;
                cards = cards2;
                cards2 = tmp;
            }

            return Array.IndexOf(cards, 2019);
        }

        public long Compute2()
        {
            long numCards = 119315717514047;
            long cardPos = 2020;

            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day22.txt").ToArray();

            for (long shuffle = 0; shuffle < 101741582076661; shuffle++)
            {
                foreach (string command in commands)
                {
                    string[] words = command.Split(' ');

                    if (words[0] == "deal")
                    {
                        if (words[1] == "into")
                        {
                            cardPos = (numCards - 1) - cardPos;
                        }
                        else
                        {
                            int increment = int.Parse(words[3]);

                            cardPos = (cardPos * increment) % numCards;
                        }
                    }
                    else if (words[0] == "cut")
                    {
                        int numToCut = int.Parse(words[1]);

                        cardPos -= numToCut;

                        if (cardPos < 0)
                            cardPos += numCards;
                        else if (cardPos >= numCards)
                            cardPos -= numCards;
                    }
                }
            }

            return cardPos;
        }
    }
}
