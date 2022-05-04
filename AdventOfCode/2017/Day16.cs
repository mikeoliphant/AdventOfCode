namespace AdventOfCode._2017
{
    internal class Day16
    {
        int size = 16;
        int[] posTransform = null;
        char[] identityTransform = null;

        void RunDance()
        {
            int startPos = 0;

            foreach (string cmd in File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day16.txt").Trim().Split(','))
            {
                switch (cmd[0])
                {
                    case 's':
                        startPos = (startPos + (size - int.Parse(cmd.Substring(1)))) % size;
                        break;

                    case 'x':
                        string[] indices = cmd.Substring(1).Split('/');

                        int index1 = (startPos + int.Parse(indices[0])) % size;
                        int index2 = (startPos + int.Parse(indices[1])) % size;

                        int tmp = posTransform[index1];
                        posTransform[index1] = posTransform[index2];
                        posTransform[index2] = tmp;

                        break;

                    case 'p':
                        string[] programs = cmd.Substring(1).Split('/');

                        int char1 = Array.IndexOf(identityTransform, programs[0][0]);
                        int char2 = Array.IndexOf(identityTransform, programs[1][0]);

                        char tmpChar = identityTransform[char1];
                        identityTransform[char1] = identityTransform[char2];
                        identityTransform[char2] = tmpChar;

                        break;

                    default:
                        throw new InvalidOperationException();
                }
            }

            int[] tmpTransform = new int[size];

            for (int pos = 0; pos < size; pos++)
            {
                tmpTransform[pos] = posTransform[(pos + startPos) % size];
            }

            posTransform = tmpTransform;
        }

        void Init()
        {
            posTransform = new int[size];
            identityTransform = new char[size];

            for (int i = 0; i < size; i++)
            {
                posTransform[i] = i;
                identityTransform[i] = (char)('a' + i);
            }
        }

        public long Compute()
        {
            Init();

            RunDance();

            string finalSequence = "";

            for (int pos = 0; pos < size; pos++)
            {
                finalSequence += identityTransform[posTransform[pos]];
            }

            return 0;
        }

        public long Compute2()
        {
            Init();

            RunDance();

            char[] sequence = new char[size];

            for (int i = 0; i < size; i++)
            {
                sequence[i] = (char)('a' + i);
            }

            char[] newSequence = new char[size];

            // Could probably look for sequence loops, but brute force is fast enough...
            for (long dance = 0; dance < 1000000000; dance++)
            {
                for (int pos = 0; pos < size; pos++)
                {
                    newSequence[pos] = identityTransform[sequence[posTransform[pos]] - 'a'];
                }

                char[] tmpSequence = sequence;
                sequence = newSequence;
                newSequence = tmpSequence;
            }

            string finalSequence = new string(sequence);

            return 0;
        }
    }
}
