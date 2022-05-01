namespace AdventOfCode._2017
{
    internal class Day1
    {

        public long Compute()
        {
            string captcha = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day1.txt").Trim();

            int startPos = 0;

            while (captcha[startPos] == captcha[(startPos + captcha.Length - 1) % captcha.Length])
            {
                startPos = (startPos + captcha.Length - 1) % captcha.Length;
            }

            int pos = startPos;
            int sum = 0;

            do
            {
                if (captcha[pos] == captcha[(pos + 1) % captcha.Length])
                {
                    sum += captcha[pos] - '0';
                }

                pos = (pos + 1) % captcha.Length;
            }
            while (pos != startPos);

            return sum;
        }

        public long Compute2()
        {
            string captcha = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2017\Day1.txt").Trim();

            int halfLength = captcha.Length / 2;

            int startPos = 0;

            while (captcha[startPos] == captcha[(startPos + captcha.Length - 1) % captcha.Length])
            {
                startPos = (startPos + captcha.Length - 1) % captcha.Length;
            }

            int pos = startPos;
            int sum = 0;

            do
            {
                if (captcha[pos] == captcha[(pos + halfLength) % captcha.Length])
                {
                    sum += captcha[pos] - '0';
                }

                pos = (pos + 1) % captcha.Length;
            }
            while (pos != startPos);

            return sum;
        }
    }
}
