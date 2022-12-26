namespace AdventOfCode._2022
{
    internal class Day20 : Day
    {
        long GetResult(long[] indices, long mult, int numMixes)
        {
            for (int i = 0; i < indices.Length; i++)
                indices[i] *= mult;

            CircularList<int> list = new CircularList<int>();

            for (int i = 0; i < indices.Length; i++)
            {
                list.Add(i);
            }

            for (int mix = 0; mix < numMixes; mix++)
            {
                for (int index = 0; index < indices.Length; index++)
                {
                    long move = indices[index];

                    var node = list.Find(index);

                    list.MoveNodeCircular(node, move);
                }
            }

            CircularList<long> restored = new CircularList<long>(list.Select(index => indices[index]));

            var zeroNode = restored.Find(0);

            return (zeroNode.MoveCircular(1000).Value + zeroNode.MoveCircular(2000).Value + zeroNode.MoveCircular(3000).Value);
        }

        public override long Compute()
        {
            //var indices = "1, 2, -3, 3, -2, 0, 4".ToLongs(',').ToArray();
            var indices = File.ReadLines(DataFile).Select(line => long.Parse(line)).ToArray();

            return GetResult(indices, 1, 1);
        }

        public override long Compute2()
        {
            //var indices = "1, 2, -3, 3, -2, 0, 4".ToLongs(',').ToArray();
            var indices = File.ReadLines(DataFile).Select(line => long.Parse(line)).ToArray();

            return GetResult(indices, 811589153, 10);
        }
    }
}
