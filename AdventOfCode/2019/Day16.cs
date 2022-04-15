namespace AdventOfCode._2019
{
    internal class Day16
    {
        int[] phasePattern = new int[] { 0, 1, 0, -1 };
        public int[] signal;

        void Process(int numPhases, int startOffset)
        {
            int halfOffset = signal.Length / 2;

            int[] newSignal = new int[signal.Length];

            for (int phase = 0; phase < 100; phase++)
            {
                int lastValue = 0;

                for (int pos = startOffset; pos < signal.Length; pos++)
                {
                    int value = 0;
                    int phaseOffset = 0;

                    if ((pos > startOffset) && (startOffset > halfOffset))  // If we're starting past halfway, everything left is multiplied by one, and each value is multiplied by one less value, so we can get (n) from (n - 1)
                    {
                        value = lastValue - signal[pos - 1];
                    }
                    else
                    {
                        for (int phasePos = startOffset; phasePos < signal.Length; phasePos++)
                        {
                            if (((phasePos + 1) % (pos + 1)) == 0)
                                phaseOffset = (phaseOffset + 1) % phasePattern.Length;

                            value += signal[phasePos] * phasePattern[phaseOffset];
                        }
                    }

                    lastValue = value;
                    newSignal[pos] = Math.Abs(value) % 10;
                }

                int[] tmp = signal;
                signal = newSignal;
                newSignal = tmp;
            }
        }

        public long Compute()
        {
            //signal = "69317163492948606335995924319873".Select(c => (int)(c - '0')).ToArray();
            signal = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day16.txt").Trim().Select(c => (int)(c - '0')).ToArray();

            Process(100, 0);

            long result = 0;

            for (int pos = 0; pos < 8; pos++)
            {
                result += signal[7 - pos] * (long)Math.Pow(10, pos);
            }

            return result;
        }


        public long Compute2()
        {
            //signal = "03036732577212944063491565474664".Select(c => (int)(c - '0')).ToArray();
            signal = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2019\Day16.txt").Trim().Select(c => (int)(c - '0')).ToArray();

            int numRepeats = 10000;

            int messageOffset = 0;

            for (int pos = 0; pos < 7; pos++)
            {
                messageOffset += signal[6 - pos] * (int)Math.Pow(10, pos);
            }

            int[] longSignal = new int[signal.Length * numRepeats];

            for (int i = 0; i < numRepeats; i++)
            {
                for (int pos = 0; pos < signal.Length; pos++)
                {
                    longSignal[(i * signal.Length) + pos] = signal[pos];
                }
            }

            signal = longSignal;

            // Digits in the signal only depend on positions equal to or greater than there own, so we save a bunch by just starting at the message offset
            Process(100, messageOffset);

            long result = 0;

            for (int pos = 0; pos < 8; pos++)
            {
                result += signal[7 - pos + messageOffset] * (long)Math.Pow(10, pos);
            }

            return result;
        }
    }
}
