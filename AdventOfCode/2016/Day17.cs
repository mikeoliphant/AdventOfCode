namespace AdventOfCode._2016
{
    internal class Day17
    {
        int gridSize = 4;
        string passcode = "pgflpeqp";

        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        string GetShortestPath(int x, int y, string pathSoFar)
        {
            //if (pathSoFar.Length > 10)
            //    return pathSoFar;

            if ((x ==(gridSize - 1)) && (y == (gridSize - 1)))
            {
                return pathSoFar;
            }

            byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(passcode + pathSoFar));
            string newState = string.Join(null, hash.Select(b => b.ToString("x2")));

            string shortest = null;
            int minLength = int.MaxValue;

            for (int i = 0; i < 4; i++)
            {
                char c = newState[i];

                if ((c < 'b') || (c > 'f')) // blocked
                    continue;

                string nextPath = null;

                switch (i)
                {
                    case 0:
                        if (y > 0)
                            nextPath = GetShortestPath(x, y - 1, pathSoFar + 'U');
                        break;

                    case 1:
                        if (y < (gridSize - 1))
                            nextPath = GetShortestPath(x, y + 1, pathSoFar + 'D');
                        break;

                    case 2:
                        if (x > 0)
                            nextPath = GetShortestPath(x - 1, y, pathSoFar + 'L');
                        break;

                    case 3:
                        if (x < (gridSize - 1))
                            nextPath = GetShortestPath(x + 1, y, pathSoFar + 'R');
                        break;
                }

                if ((nextPath != null) && (nextPath.Length < minLength))
                {
                    shortest = nextPath;
                    minLength = nextPath.Length;
                }
            }

            return shortest;
        }

        string GetLongestPath(int x, int y, string pathSoFar)
        {
            //if (pathSoFar.Length > 10)
            //    return pathSoFar;

            if ((x == (gridSize - 1)) && (y == (gridSize - 1)))
            {
                return pathSoFar;
            }

            byte[] hash = md5.ComputeHash(System.Text.Encoding.ASCII.GetBytes(passcode + pathSoFar));
            string newState = string.Join(null, hash.Select(b => b.ToString("x2")));

            string longest = null;
            int maxLength = int.MinValue;

            for (int i = 0; i < 4; i++)
            {
                char c = newState[i];

                if ((c < 'b') || (c > 'f')) // blocked
                    continue;

                string nextPath = null;

                switch (i)
                {
                    case 0:
                        if (y > 0)
                            nextPath = GetLongestPath(x, y - 1, pathSoFar + 'U');
                        break;

                    case 1:
                        if (y < (gridSize - 1))
                            nextPath = GetLongestPath(x, y + 1, pathSoFar + 'D');
                        break;

                    case 2:
                        if (x > 0)
                            nextPath = GetLongestPath(x - 1, y, pathSoFar + 'L');
                        break;

                    case 3:
                        if (x < (gridSize - 1))
                            nextPath = GetLongestPath(x + 1, y, pathSoFar + 'R');
                        break;
                }

                if ((nextPath != null) && (nextPath.Length > maxLength))
                {
                    longest = nextPath;
                    maxLength = nextPath.Length;
                }
            }

            return longest;
        }

        public long Compute()
        {
            string path = GetShortestPath(0, 0, "");

            return 0;
        }


        public long Compute2()
        {
            string path = GetLongestPath(0, 0, "");

            return path.Length;
        }
    }
}
