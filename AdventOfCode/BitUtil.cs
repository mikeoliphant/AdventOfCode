namespace AdventOfCode
{
    public static class BitUtil
    {
        public static long SetLongStorage(long storage, long value, int offset, int numBits)
        {
            long mask = (long)((~((ulong)0) >> (64 - numBits)) << offset);

            storage &= ~mask;

            return storage | (value << offset);
        }

        public static long GetLongStorage(long storage, int offset, int numBits)
        {
            return (((1 << numBits) - 1) & (storage >> offset));
        }

        public static int NumberOfSetBits(int i)
        {
            i = i - ((i >> 1) & 0x55555555);
            i = (i & 0x33333333) + ((i >> 2) & 0x33333333);

            return (((i + (i >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }
    }
}
