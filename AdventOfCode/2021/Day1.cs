namespace AdventOfCode._2021
{
    public class Day1
    {
        List<int> ReadValues()
        {
            List<int> values = new List<int>();

            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2021\DayOneInput.txt"))
            {
                do
                {
                    string line = reader.ReadLine();

                    if (line == null)
                        break;

                    values.Add(int.Parse(line));
                }
                while (true);
            }

            return values;
        }

        public void Compute()
        {
            int lastValue = 0;
            int numValues = 0;
            int numIncreasing = 0;

            bool first = true;

            List<int> values = ReadValues();

            foreach (int value in values)
            {
                numValues++;

                if (first)
                {
                    first = false;
                }
                else
                {
                    if (value > lastValue)
                        numIncreasing++;
                }

                lastValue = value;
            }
        }

        public void Compute2()
        {
            int lastValue = 0;
            int numValues = 0;
            int numIncreasing = 0;

            bool first = true;

            int windowSize = 3;

            List<int> window = new List<int>();

            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\DayOneInput.txt"))
            {
                List<int> values = ReadValues();

                foreach (int value in values)
                {
                    numValues++;

                    window.Add(value);

                    if (window.Count > windowSize)
                    {
                        window.RemoveAt(0);
                    }

                    if (window.Count < 3)
                        continue;

                    int sum = 0;

                    foreach (int val in window)
                    {
                        sum += val;
                    }


                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        if (sum > lastValue)
                            numIncreasing++;
                    }

                    lastValue = sum;
                }
            }
        }
    }
}
