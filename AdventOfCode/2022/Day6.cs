namespace AdventOfCode._2022
{
    internal class Day6 : Day
    {
        public long GetPacketOffset(string data, int numUnique)
        {
            CountHash<char> countHash = new CountHash<char>();

            for (int pos = 0; pos < data.Length; pos++)
            {
                if (pos >= numUnique)
                {
                    countHash.Decrement(data[pos - numUnique]);
                }

                countHash.Increment(data[pos]);

                if (countHash.NumNonzero == numUnique)
                    return (pos + 1);
            }

            throw new Exception();
        }

        public override long Compute()
        {
            string data = File.ReadAllText(DataFile);

            return GetPacketOffset(data, 4);
        }

        public override long Compute2()
        {
            string data = File.ReadAllText(DataFile);

            return GetPacketOffset(data, 14);
        }
    }
}
