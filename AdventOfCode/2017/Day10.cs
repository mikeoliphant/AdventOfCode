namespace AdventOfCode._2017
{
    internal class Day10
    {
        void RunCycle()
        {

        }

        public long Compute()
        {
            //int[] lengths = new int[] { 3, 4, 1, 5 };
            int[] lengths = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day10.txt").Trim().ToInts(',').ToArray();

            int size = 256;

            int[] list = new int[size];

            for (int i = 0; i < size; i++)
            {
                list[i] = i;
            }

            int skip = 0;
            int pos = 0;

            for (int length = 0; length < lengths.Length; length++)
            {
                int reverseSize = lengths[length] / 2;

                for (int reverse = 0; reverse < reverseSize; reverse++)
                {
                    int index1 = (pos + reverse) % size;
                    int index2 = (pos + lengths[length] - reverse - 1) % size;

                    int tmp = list[index1];
                    list[index1] = list[index2];
                    list[index2] = tmp;
                }

                pos = (pos + lengths[length] + skip) % size;
                skip++;
            }

            return list[0] * list[1];
        }

        public long Compute2()
        {
            //string lengthStr = "1,2,3";
            string lengthStr = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day10.txt").Trim();

            List<int> lengths = lengthStr.Select(c => (int)c).ToList();
            lengths.AddRange(new int[] { 17, 31, 73, 47, 23 });

            int size = 256;

            int[] list = new int[size];

            for (int i = 0; i < size; i++)
            {
                list[i] = i;
            }

            int skip = 0;
            int pos = 0;

            for (int round = 0; round < 64; round++)
            {
                for (int length = 0; length < lengths.Count; length++)
                {
                    int reverseSize = lengths[length] / 2;

                    for (int reverse = 0; reverse < reverseSize; reverse++)
                    {
                        int index1 = (pos + reverse) % size;
                        int index2 = (pos + lengths[length] - reverse - 1) % size;

                        int tmp = list[index1];
                        list[index1] = list[index2];
                        list[index2] = tmp;
                    }

                    pos = (pos + lengths[length] + skip) % size;
                    skip++;
                }
            }

            int[] denseHash = new int[16];

            for (int hash = 0; hash < 16; hash++)
            {
                denseHash[hash] = 0;
                int offset = hash * 16;

                for (int hashPos = 0; hashPos < 16; hashPos++)
                {
                    denseHash[hash] ^= list[offset + hashPos];
                }
            }

            string hashStr = string.Join("", denseHash.Select(h => h.ToString("X"))).ToLower();

            return 0;
        }
    }
}
