
namespace AdventOfCode._2025
{
    internal class Day8 : Day
    {
        int GetRootConnect(int[] connect, int id)
        {            
            while (connect[id] != id)
            {
                id = connect[id];
            }

            return id;
        }



        public override long Compute()
        {
            List<Vector3> boxes = new();

            foreach (string line in File.ReadLines(DataFile))
            {
                boxes.Add(new Vector3(line.ToFloats(',').ToArray()));
            }

            List<(int, int)> pairs = new();

            for (int i = 0; i < boxes.Count; i++)
            {
                for (int j = i + 1; j < boxes.Count; j++)
                {
                    pairs.Add((i, j));
                }
            }

            var sorted = pairs.OrderBy(p => Vector3.Distance(boxes[p.Item1], boxes[p.Item2]));

            var connect = Enumerable.Range(0, boxes.Count).ToArray();

            foreach (var pair in sorted.Take(1000))
            {
                int root1 = GetRootConnect(connect, pair.Item1);
                int root2 = GetRootConnect(connect, pair.Item2);

                connect[root1] = root2;
            }

            var roots = connect.Select(c => GetRootConnect(connect, c));

            var counts = roots.GroupBy(r => r).Select(g => (g.Key, g.Count())).OrderByDescending(g => g.Item2);

            int product = 1;

            foreach (var count in counts.Take(3))
            {
                product *= count.Item2;
            }

            return product;
        }

        public override long Compute2()
        {
            List<Vector3> boxes = new();

            foreach (string line in File.ReadLines(DataFile))
            {
                boxes.Add(new Vector3(line.ToFloats(',').ToArray()));
            }

            List<(int, int)> pairs = new();

            for (int i = 0; i < boxes.Count; i++)
            {
                for (int j = i + 1; j < boxes.Count; j++)
                {
                    pairs.Add((i, j));
                }
            }

            var sorted = pairs.OrderBy(p => Vector3.Distance(boxes[p.Item1], boxes[p.Item2]));

            var connect = Enumerable.Range(0, boxes.Count).ToArray();

            foreach (var pair in sorted)
            {
                int root1 = GetRootConnect(connect, pair.Item1);
                int root2 = GetRootConnect(connect, pair.Item2);

                connect[root1] = root2;

                var roots = connect.Select(c => GetRootConnect(connect, c));

                var groups = roots.GroupBy(r => r);

                if (groups.Count() == 1)
                {
                    return (long)boxes[pair.Item1].X * (long)boxes[pair.Item2].X;
                }
            }

            throw new InvalidOperationException();
        }
    }
}
