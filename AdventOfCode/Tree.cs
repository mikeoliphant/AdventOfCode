namespace AdventOfCode
{
    public class TreeNode<T>
    {
        public List<TreeNode<T>> Children { get; set; } = new List<TreeNode<T>>();
        public T Value { get; set; } = default(T);

        public TreeNode()
        {
        }

        public TreeNode(T value)
            : base()
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return ((Value == null) ? "<null>" : Value.ToString()) + " => [" + Children.Count + "]";
        }

        public void PrintToConsole()
        {
            PrintNode("", last: true);

            Console.WriteLine();
        }

        void PrintNode(string indent, bool last)
        {
            Console.Write(indent);

            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            Console.WriteLine(this.ToString());

            for (int i = 0; i < Children.Count; i++)
                Children[i].PrintNode(indent, i == Children.Count - 1);
        }
    }
}
