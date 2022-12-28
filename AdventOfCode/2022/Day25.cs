namespace AdventOfCode._2022
{
    internal class Day25 : Day
    {
        long GetSnafu(string snafu)
        {
            long sum = 0;

            for (int pos = 0; pos < snafu.Length; pos++)
            {
                long mult = (long)Math.Pow(5, snafu.Length - pos - 1);

                switch (snafu[pos])
                {
                    case '0':
                        mult = 0;
                        break;
                    case '1':                        
                        break;
                    case '2':
                        mult *= 2;
                        break;
                    case '-':
                        mult *= -1;
                        break;
                    case '=':
                        mult *= -2;
                        break;
                }

                sum += mult;
            }

            return sum;
        }

        string ToSnafu(long dec)
        {
            long abs = Math.Abs(dec);

            if (abs < 3)
            {
                if (dec >= 0)
                    return dec.ToString();
                else if (dec == -1)
                    return "-";
                else
                    return "=";
            }

            int pow = (int)(Math.Log(abs) / Math.Log(5));

            int val = (int)Math.Round(abs / (Math.Pow(5, pow)));

            if (val > 2)
            {
                pow++;
                val = 1;
            }

            val *= Math.Sign(dec);

            long rem = dec - (val * (long)Math.Pow(5, pow));

            return ToSnafu(val) + ToSnafu(rem).PadLeft(pow, '0');
        }

        public override long Compute()
        {
            //long val = GetSnafu("1121-1110-1=0");

            long sum = 0;

            foreach (string snafu in File.ReadLines(DataFile))
            {
                sum += GetSnafu(snafu);
            }

            string snafuSum = ToSnafu(sum);

            return base.Compute();
        }
    }
}
