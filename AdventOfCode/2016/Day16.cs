namespace AdventOfCode._2016
{
    internal class Day16
    {
        BitArray Expand(BitArray bits)
        {
            int newLength = (bits.Length * 2) +1;

            BitArray newBits = new BitArray(newLength);

            for (int pos = 0; pos < bits.Length; pos++)
            {
                newBits[pos] = bits[pos];
                newBits[newLength - pos - 1] = !bits[pos];
                newBits[bits.Length] = false;
            }

            return newBits;
        }

        BitArray Checksum(BitArray bits)
        {
            BitArray newBits = new BitArray(bits.Length / 2);

            for (int pos = 0; pos < bits.Length; pos += 2)
            {
                newBits[pos / 2] = bits[pos] == bits[pos + 1];
            }

            return newBits;
        }

        public long Compute()
        {
            //string startVal = "10000";
            //int desiredSize = 20;
            string startVal = "10010000000110000";
            int desiredSize = 35651584;// 272;

            BitArray bits = new BitArray(startVal.Select(b => b == '1').ToArray());

            while (bits.Length < desiredSize)
            {
                bits = Expand(bits);
            }

            bits = new BitArray(bits.Cast<bool>().Take(desiredSize).ToArray());

            do
            {
                bits = Checksum(bits);
            }
            while ((bits.Length % 2) == 0);

            string bitString = new String(bits.Cast<bool>().Select(b => b ? '1' : '0').ToArray());

            return 0;
        }
    }
}
