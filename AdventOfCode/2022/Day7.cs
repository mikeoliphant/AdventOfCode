namespace AdventOfCode._2022
{
    class FileNode
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public bool IsDirectory { get; set; }

        public override string ToString()
        {
            return Name + " : " + Size;
        }
    }

    internal class Day7 : Day
    {
        TreeNode<FileNode> ParseFilesystem()
        {
            TreeNode<FileNode> rootNode = new TreeNode<FileNode>(new FileNode { Name = "/", IsDirectory = true });
            TreeNode<FileNode> currentNode = rootNode;

            Stack<TreeNode<FileNode>> path = new Stack<TreeNode<FileNode>>();

            foreach (string line in File.ReadLines(DataFile))
            {
                string[] words = line.Split(' ');

                if (words[0] == "$")
                {
                    switch (words[1])
                    {
                        case "cd":
                            if (words[2] == "/")
                            {
                                currentNode = rootNode;
                            }
                            else if (words[2] == "..")
                            {
                                currentNode = path.Pop();
                            }
                            else
                            {
                                path.Push(currentNode);
                                currentNode = currentNode.Children.FirstOrDefault(c => c.Value.Name == words[2]);
                            }
                            break;

                        case "ls":
                            break;
                    }
                }
                else
                {
                    if (words[0] == "dir")
                    {
                        currentNode.Children.Add(new TreeNode<FileNode>(new FileNode { Name = words[1], IsDirectory = true }));
                    }
                    else
                    {
                        currentNode.Children.Add(new TreeNode<FileNode>(new FileNode { Size = long.Parse(words[0]), Name = words[1] }));
                    }
                }
            }

            return rootNode;
        }

        long AddDirSize(TreeNode<FileNode> node)
        {
            if (node.Value.IsDirectory)
                node.Value.Size = node.Children.Sum(c => AddDirSize(c));

            return node.Value.Size;
        }

        public override long Compute()
        {
            var fs = ParseFilesystem();

            AddDirSize(fs);

            //fs.PrintToConsole();

            long dirSum = 0;

            fs.RunNodeAction(delegate (TreeNode<FileNode> node)
            {
                if (node.Value.IsDirectory && (node.Value.Size <= 100000))
                {
                    dirSum += node.Value.Size;
                }
            });

            return dirSum;
        }

        public override long Compute2()
        {
            var fs = ParseFilesystem();

            AddDirSize(fs);

            //fs.PrintToConsole();

            long spaceFree = 70000000 - fs.Value.Size;

            long spaceNeeded = 30000000 - spaceFree;

            FileNode smallest = null;

            fs.RunNodeAction(delegate (TreeNode<FileNode> node)
            {
                if (node.Value.IsDirectory && (node.Value.Size >= spaceNeeded))
                {
                    if ((smallest == null) || (smallest.Size > node.Value.Size))
                    {
                        smallest = node.Value;
                    }
                }
            });

            return smallest.Size;
        }
    }
}