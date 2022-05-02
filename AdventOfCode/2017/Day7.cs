namespace AdventOfCode._2017
{
    class Program
    {
        public string Name { get; set; }
        public int Weight { get; set; }

        public override string ToString()
        {
            return Name + "(" + Weight + ")";
        }
    }

    internal class Day7
    {
        TreeNode<Program> tree;
        Dictionary<string, TreeNode<Program>> dict = new Dictionary<string, TreeNode<Program>>();

        TreeNode<Program> GetNode(string name)
        {
            if (!dict.ContainsKey(name))
            {
                dict[name] = new TreeNode<Program>(new Program { Name = name });
            }

            return dict[name];
        }

        void ReadInput()
        {
            foreach (string nodeStr in File.ReadLines(@"C:\Code\AdventOfCode\Input\2017\Day7.txt"))
            {
                string[] split = nodeStr.Split(" -> ");

                string[] nameWeight = split[0].Split(' ');

                string name = nameWeight[0];
                int weight = int.Parse(nameWeight[1].Substring(1, nameWeight[1].Length - 2));

                TreeNode<Program> program = GetNode(name);

                program.Value.Weight = weight;
                
                if (split.Length > 1)
                {
                    foreach (string child in split[1].Split(", "))
                    {
                        program.Children.Add(GetNode(child));
                    }
                }
            }

            tree = dict.Where(n => (n.Value.Children.Count > 0) && !dict.Values.Where(b => b.Children.Contains(n.Value)).Any()).First().Value;
        }

        int TotalWeight(TreeNode<Program> node)
        {
            return node.Value.Weight + node.Children.Sum(c => TotalWeight(c));
        }

        bool Balance(TreeNode<Program> node)
        {
            var groups = node.Children.GroupBy(c => TotalWeight(c)).OrderBy(g => g.Count());

            if (groups.Count() == 1)
                return true;

            int desiredWeight = groups.Skip(1).First().Key;

            var oddOneOut = groups.First().First();

            if (Balance(oddOneOut))
            {
                int diff = desiredWeight - TotalWeight(oddOneOut);

                if (diff != 0)
                {
                    oddOneOut.Value.Weight += diff;

                    neededWeight = oddOneOut.Value.Weight;
                }

                return true;
            }

            return false;
        }

        int neededWeight = 0;

        public long Compute()
        {
            ReadInput();

            tree.PrintToConsole();

            string bottomName = tree.Value.Name;

            Balance(tree);

            return neededWeight;
        }
    }
}
