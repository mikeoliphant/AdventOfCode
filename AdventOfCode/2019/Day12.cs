using System.Reflection;

namespace AdventOfCode._2019
{
    class Moon
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public override string ToString()
        {
            return Position.ToString() + " | " + Velocity.ToString();
        }
    }

    internal class Day12
    {
        List<Moon> moons = new List<Moon>();
        List<Moon> initialMoons = new List<Moon>();

        public void ReadInput()
        {
            foreach (string moonStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2019\Day12.txt"))
            {
                string[] coords = moonStr.Replace("<", "").Replace(">", "").Split(',');

                moons.Add(new Moon { Position = new Vector3(int.Parse(coords[0].Split('=')[1]), int.Parse(coords[1].Split('=')[1]), int.Parse(coords[2].Split('=')[1])) });
                initialMoons.Add(new Moon { Position = new Vector3(int.Parse(coords[0].Split('=')[1]), int.Parse(coords[1].Split('=')[1]), int.Parse(coords[2].Split('=')[1])) });
            }
        }

        void Simulate()
        {
            for (int moon1 = 0; moon1 < moons.Count; moon1++)
            {
                for (int moon2 = moon1 + 1; moon2 < moons.Count; moon2++)
                {
                    if (moon1 != moon2)
                    {
                        Vector3 diff = moons[moon2].Position - moons[moon1].Position;

                        diff = new Vector3(Math.Sign(diff.X), Math.Sign(diff.Y), Math.Sign(diff.Z));

                        moons[moon1].Velocity += diff;
                        moons[moon2].Velocity -= diff;
                    }
                }
            }

            foreach (Moon moon in moons)
            {
                moon.Position += moon.Velocity;
            }
        }

        void PrintMoons()
        {
            foreach (Moon moon in moons)
            {
                Console.WriteLine(moon.ToString());
            }

            Console.WriteLine();
        }

        public long Compute()
        {
            ReadInput();

            for (int i = 0; i < 1000; i++)
            {
                Simulate();
            }

            int energy = 0;

            foreach (Moon moon in moons)
            {
                energy += (int)(Math.Abs(moon.Position.X) + Math.Abs(moon.Position.Y) + Math.Abs(moon.Position.Z)) * (int)(Math.Abs(moon.Velocity.X) + Math.Abs(moon.Velocity.Y) + Math.Abs(moon.Velocity.Z));
            }

            return energy;
        }

        bool CheckMatch(string axis)
        {
            FieldInfo field = moons[0].Position.GetType().GetField(axis);

            for (int pos = 0; pos < moons.Count; pos++)
            {
                if ((float)field.GetValue(moons[pos].Position) != (float)field.GetValue(initialMoons[pos].Position))
                {
                    return false;
                }

                if ((float)field.GetValue(moons[pos].Velocity) != (float)field.GetValue(initialMoons[pos].Velocity))
                {
                    return false;
                }
            }

            return true;
        }

        int GetPeriod(string axis)
        {
            int step = 0;

            do
            {
                Simulate();

                step++;
            }
            while (!CheckMatch(axis));

            for (int pos = 0; pos < moons.Count; pos++)
            {
                moons[pos].Position = initialMoons[pos].Position;
                moons[pos].Velocity = initialMoons[pos].Velocity;
            }

            return step;
        }

        public long Compute2()
        {
            ReadInput();

            int[] periods = new int[3];

            periods[0] = GetPeriod("X");
            periods[1] = GetPeriod("Y");
            periods[2] = GetPeriod("Z");

            int max = 0;
            int maxPeriod = 0;

            for (int i = 0; i < 3; i++)
            {
                if (periods[i] > maxPeriod)
                {
                    maxPeriod = periods[i];
                    max = i;
                }
            }

            long step = 0;

            do
            {
                step += maxPeriod;

                bool allMatch = true;

                for (int i = 0; i < 3; i++)
                {
                    if (i != max)
                    {
                        if ((((long)(step / periods[i])) * periods[i]) != step)
                        {
                            allMatch = false;

                            break;
                        }
                    }
                }

                if (allMatch)
                    break;
            }
            while (true);

            return 0;
        }
    }
}
