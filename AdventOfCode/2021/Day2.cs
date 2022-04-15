namespace AdventOfCode._2021
{
    public class Day2
    {

        List<string> ReadCommands()
        {
            List<string> commands = new List<string>();

            using (StreamReader reader = new StreamReader(@"C:\Code\AdventOfCode\Input\2021\DayTwoInput.txt"))
            {
                do
                {
                    string line = reader.ReadLine();

                    if (line == null)
                        break;

                    commands.Add(line);
                }
                while (true);
            }

            return commands;
        }

        public void Compute()
        {
            List<string> commands = ReadCommands();

            int horizontalDistance = 0;
            int depth = 0;

            foreach (string command in commands)
            {
                string[] split = command.Split(' ');

                int distance = int.Parse(split[1]);

                if (split[0] == "forward")
                {
                    horizontalDistance += distance;
                }
                else if (split[0] == "down")
                {
                    depth += distance;
                }
                else if (split[0] == "up")
                {
                    depth -= distance;
                }
            }

            int mult = horizontalDistance * depth;
        }

        public void Compute2()
        {
            List<string> commands = ReadCommands();

            int aim = 0;
            int horizontalDistance = 0;
            int depth = 0;

            foreach (string command in commands)
            {
                string[] split = command.Split(' ');

                int distance = int.Parse(split[1]);

                if (split[0] == "forward")
                {
                    horizontalDistance += distance;
                    depth += aim * distance;
                }
                else if (split[0] == "down")
                {
                    aim += distance;
                }
                else if (split[0] == "up")
                {
                    aim -= distance;
                }
            }

            int mult = horizontalDistance * depth;
        }

    }
}
