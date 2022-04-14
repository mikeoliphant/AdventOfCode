using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode._2019
{
    internal class Day22
    {
        public long Compute()
        {
            int numCards = 100007;

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

        static BigInteger ModInverse(BigInteger a, BigInteger m)
        {
            BigInteger m0 = m;
            BigInteger y = 0, x = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                // q is quotient
                BigInteger q = a / m;

                BigInteger t = m;

                // m is remainder now, process
                // same as Euclid's algo
                m = a % m;
                a = t;
                t = y;

                // Update x and y
                y = x - q * y;
                x = t;
            }

            // Make x positive
            if (x < 0)
                x += m0;

            return x;
        }

        public long ModDiv(long numCards, BigInteger pos, BigInteger div)
        {
            BigInteger inv = ModInverse(div, numCards);

            BigInteger divisible = pos * inv;

            return (long)(divisible % numCards);
        }

        public long IterativForwardShuffle(long numCards, BigInteger startPos, BigInteger mult, BigInteger offset, BigInteger numShuffles)
        {
            for (long shuffle = 0; shuffle < numShuffles; shuffle++)
            {
                startPos = Mod((startPos * mult) + offset, numCards);
            }

            return (long)startPos;
        }

        public long IterativeReverseShuffle(long numCards, BigInteger endPos, BigInteger mult, BigInteger offset, BigInteger numShuffles)
        {
            for (long shuffle = 0; shuffle < numShuffles; shuffle++)
            {
                endPos = ModDiv(numCards, endPos - offset, mult);
            }

            if (endPos < 0)
                endPos += numCards;

            return (long)endPos;
        }

        public long ComputeReverseShuffle(long numCards, BigInteger endPos, BigInteger mult, BigInteger offset, BigInteger numShuffles)
        {
            //BigInteger calcCardInPos = (2020 + (offset * ((BigInteger.ModPow(mult, numShuffles, numCards) - 1) / (mult - 1)))) / BigInteger.ModPow(mult, numShuffles, numCards);

            BigInteger modPow = BigInteger.ModPow(mult, numShuffles, numCards);

            //endPos = ModDiv(numCards, (endPos + (offset * ModDiv(numCards, modPow - 1, (mult - 1)))), modPow);

            BigInteger div = (long)ModDiv(numCards, modPow - 1, (mult - 1));

            endPos = (long)ModDiv(numCards, (endPos - Mod(offset * div, numCards)), modPow);

            return Mod(endPos, numCards);
        }

        public long Mod(BigInteger num, BigInteger mod)
        {
            num = num % mod;

            if (num < 0)
                num += mod;

            return (long)num;
        }

        public long Compute2()
        {
            long numCards = 119315717514047;

            string[] commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day22.txt").ToArray();

            long mult = 1;
            long offset = 0;

            foreach (string command in commands)
            {
                string[] words = command.Split(' ');

                if (words[0] == "deal")
                {
                    if (words[1] == "into")
                    {
                        mult = -mult;
                        offset = (numCards - 1) - offset;
                    }
                    else
                    {
                        int increment = int.Parse(words[3]);

                        mult *= increment;
                        offset *= increment;

                        mult = Mod(mult, numCards);
                        offset = Mod(offset, numCards);
                    }
                }
                else if (words[0] == "cut")
                {
                    int numToCut = int.Parse(words[1]);

                    offset -= numToCut;
                }
            }

            //
            // endPos = (startPos * mult) + offset
            // startPos = (endPos - offset) / mult
            //
            // startPos(t) = (startPos(t - 1) - offset) / mult
            //

            long numShuffles = 101741582076661;

            //BigInteger blah = (BigInteger.ModPow(mult, numShuffles, numCards) - 1);

            //BigInteger calcCardInPos = (2020 + (offset * ((BigInteger.ModPow(mult, numShuffles, numCards) - 1) / (mult - 1)))) / BigInteger.ModPow(mult, numShuffles, numCards);

            long startCard = numCards - 1;

            //long endPos = IterativForwardShuffle(numCards, startCard, mult, offset, 10000000);

            //long initialCard = IterativeReverseShuffle(numCards, endPos, mult, offset, 100);


            //long initialCard = ComputeReverseShuffle(numCards, endPos, mult, offset, 10000000);

            long initialCard = ComputeReverseShuffle(numCards, 2020, mult, offset, numShuffles);

            //if (initialCard != startCard)
            //    throw new Exception();

            return 0;
        }
    }
}
