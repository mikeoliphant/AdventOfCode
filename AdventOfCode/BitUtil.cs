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
    }
}
