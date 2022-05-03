namespace AdventOfCode._2017
{
    internal class Day12
    {
        Dictionary<int, TreeNode<int>> dict = new Dictionary<int, TreeNode<int>>();

        TreeNode<int> GetNode(int node)
        {
            if (!dict.ContainsKey(node))
                dict[node] = new TreeNode<int>(node);

            return dict[node];
        }

        HashSet<int> ReacheableNodes(TreeNode<int> node)
        {
            return ReacheableNodes(node, new HashSet<int>());
        }

        HashSet<int> ReacheableNodes(TreeNode<int> node, HashSet<int> nodesSoFar)
        {
            nodesSoFar.Add(node.Value);

            foreach (var connectedNode in node.Children)
            {
                if (!nodesSoFar.Contains(connectedNode.Value))
                {
                    foreach (int reach in ReacheableNodes(connectedNode, nodesSoFar))
                    {
                        nodesSoFar.Add(reach);
                    }
                }
            }

            return nodesSoFar;
        }

        void ReadInput()
        {
            foreach (string connection in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day12.txt"))
            {
                string[] split = connection.Split(" <-> ");

                var node = GetNode(int.Parse(split[0]));

                foreach (int connectedNode in split[1].ToInts(','))
                {
                    node.Children.Add(GetNode(connectedNode));
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            var reacheable = ReacheableNodes(GetNode(0));

            return reacheable.Count;
        }

        public long Compute2()
        {
            ReadInput();

            HashSet<int> available = new HashSet<int>(dict.Keys);

            int numGroups = 0;

            while (available.Count > 0)
            {
                var reacheable = ReacheableNodes(GetNode(available.First()));

                numGroups++;

                foreach (int node in reacheable)
                {
                    available.Remove(node);
                }
            }

            return numGroups;
        }
    }
}
