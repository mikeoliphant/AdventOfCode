namespace AdventOfCode._2021
{
    public class Day8
    {
        string[] inputs;
        string[] outputs;
        string[] inouts;

        int[] numberBits = new int[] { 0b01110111, 0b00010010, 0b01011101, 0b01011011, 0b00111010, 0b01101011, 0b01101111, 0b01010010, 0b01111111, 0b01111011 };
        int[] numberNumBits = new int[10];
        int[] numBitBitmasks = new int[8];

        public Day8()
        {
            for (int number = 0; number < 10; number++)
            {
                for (int bit = 0; bit < 8; bit++)
                {
                    if ((numberBits[number] & (1 << bit)) != 0)
                    {
                        numberNumBits[number]++;
                    }
                }
            }

            for (int number = 0; number < 10; number++)
            {
                numBitBitmasks[numberNumBits[number]] |= numberBits[number];
            }
        }

        void ReadData()
        {
            string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day8.txt").ToArray();
            //string[] lines = File.ReadLines(@"C:\Code\AdventOfCode\Input\2021\Day8Test.txt").ToArray();
            //string[] lines = new string[] { "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf" };

            inputs = new string[lines.Length];
            outputs = new string[lines.Length];
            inouts = new string[lines.Length];

            for (int pos = 0; pos < lines.Length; pos++)
            {
                string[] inout = lines[pos].Split(" | ");

                inouts[pos] = inout[0] + " " + inout[1];
                inputs[pos] = inout[0];
                outputs[pos] = inout[1];
            }
        }

        public long Compute()
        {
            ReadData();

            int[] segmentHistogram = new int[8];

            foreach (string output in outputs)
            {
                string[] patterns = output.Split(' ');

                foreach (string pattern in patterns)
                {
                    segmentHistogram[pattern.Length]++;
                }
            }

            return segmentHistogram[2] + segmentHistogram[4] + segmentHistogram[3] + segmentHistogram[7];
        }

        int DecodeNumber(string pattern, Dictionary<char, int> valMap)
        {
            int bits = 0;

            foreach (char c in pattern)
            {
                bits |= valMap[c];
            }

            return bits;
        }

        Dictionary<char, int> BruteForce(string input, Dictionary<char, int> valMap)
        {
            bool recursed = false;

            for (char c = 'a'; c <= 'g'; c++)
            {
                int possibleValues = valMap[c];

                if ((possibleValues & (possibleValues - 1)) != 0)  // Trick to check if more than one bit is set
                {
                    for (int bit = 0; bit < 8; bit++)
                    {
                        if ((possibleValues & (1 << bit)) != 0)
                        {
                            recursed = true;

                            Dictionary<char, int> newMap = new Dictionary<char, int>(valMap);

                            for (char c2 = 'a'; c2 <= 'g'; c2++)
                            {
                                newMap[c2] = (c2 == c) ? (1 << bit) : (newMap[c2] & ~(1 << bit));
                            }

                            Dictionary<char, int> result = BruteForce(input, newMap);

                            if (result != null)
                                return result;
                        }
                    }
                }
            }

            if (recursed)
                return null;

            // Check validity

            foreach (string pattern in input.Split(' '))
            {
                int bits = DecodeNumber(pattern, valMap);

                bool isValid = false;

                foreach (int nBits in numberBits)
                {
                    if (bits == nBits)
                    {
                        isValid = true;
                        break;
                    }
                }

                if (!isValid)
                    return null;
            }

            return valMap;
        }

        Dictionary<char, int> Decode(string input)
        {
            Dictionary<char, int> valMap = new Dictionary<char, int>();

            for (char c = 'a'; c <= 'g'; c++)
            {
                valMap[c] = 0b01111111;
            }

            //return BruteForce(input, valMap);

            string[] lengthChars = new string[8];

            foreach (string pattern in input.Split(' '))
            {
                int numLetters = pattern.Length;

                if (lengthChars[numLetters] == null)
                    lengthChars[numLetters] = "";

                foreach (char c in pattern)
                {
                    if (!lengthChars[numLetters].Contains(c))
                        lengthChars[numLetters] += c;
                }
            }

            for (int numLetters = 0; numLetters < 8; numLetters++)
            {
                if (lengthChars[numLetters] != null)
                {
                    for (char c = 'a'; c <= 'g'; c++)
                    {
                        if (lengthChars[numLetters].Contains(c))
                        {
                            valMap[c] &= numBitBitmasks[numLetters];
                        }
                        else
                        {
                            valMap[c] &= ~numBitBitmasks[numLetters];
                        }
                    }
                }
            }

            return BruteForce(input, valMap);
        }

        public long Compute2()
        {
            ReadData();

            long sum = 0;

            for (int pos = 0; pos < inouts.Length; pos++)
            {
                Dictionary<char, int> valMap = Decode(inouts[pos]);

                string outputDigits = "";

                foreach (string output in outputs[pos].Split(' '))
                {
                    int value = DecodeNumber(output, valMap);

                    outputDigits += Array.IndexOf(numberBits, value).ToString();
                }

                sum += int.Parse(outputDigits);
            }

            return sum;
        }
    }
}
