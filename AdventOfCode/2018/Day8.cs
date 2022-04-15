namespace AdventOfCode._2018
{
    internal class Day8
    {
        class TreeNode
        {
            public TreeNode[] Children { get; set; }
            public int[] MetaData { get; set;  }

            public long GetMetaDataSum()
            {
                return MetaData.Sum() + Children.Sum(c => c.GetMetaDataSum());
            }

            public long GetValue()
            {
                long sum = 0;

                if (Children.Length == 0)
                    return MetaData.Sum();
                else
                {
                    foreach (int childID in MetaData)
                    {
                        if (childID <= Children.Length)
                        {
                            sum += Children[childID - 1].GetValue();
                        }
                    }
                }

                return sum;
            }
        }

        TreeNode tree;

        TreeNode AddNode(int[] data, int pos, out int endPos)
        {
            TreeNode node = new TreeNode();

            node.Children = new TreeNode[data[pos++]];
            node.MetaData = new int[data[pos++]];

            for (int child = 0; child < node.Children.Length; child++)
            {
                node.Children[child] = AddNode(data, pos, out pos);
            }

            Array.Copy(data, pos, node.MetaData, 0, node.MetaData.Length);

            endPos = pos + node.MetaData.Length;

            return node;
        }

        void ReadInput()
        {
            int[] data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2018\Day8.txt").Trim().ToInts(' ').ToArray();

            int endPos;

            tree = AddNode(data, 0, out endPos);
        }

        public long Compute()
        {
            ReadInput();

            return tree.GetMetaDataSum();
        }

        public long Compute2()
        {
            ReadInput();

            return tree.GetValue();
        }
    }
}
