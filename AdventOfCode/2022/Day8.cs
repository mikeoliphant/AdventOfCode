namespace AdventOfCode._2022
{
    internal class Day8 : Day
    {
        Grid<char> treeGrid;
        Dictionary<(int X, int Y), bool> visibleDict = new Dictionary<(int X, int Y), bool>();

        int CountVisibleTrees((int X, int Y) pos, (int DX, int DY) dir)
        {
            int count = 0;

            char maxHeight = (char)('0' - 1);

            do
            {
                char c;

                if (!treeGrid.TryGetValue(pos.X, pos.Y, out c))
                {
                    return count;
                }

                if (c > maxHeight)
                {
                    visibleDict[pos] = true;

                    count++;

                    maxHeight = c;
                }

                pos.X += dir.DX;
                pos.Y += dir.DY;
            }
            while (true);
        }

        public override long Compute()
        {
            treeGrid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));
            
            // From top
            for (int x = 0; x < treeGrid.Width; x++)
                CountVisibleTrees((x, 0), (0, 1));

            // From bottom
            for (int x = 0; x < treeGrid.Width; x++)
                CountVisibleTrees((x, treeGrid.Height - 1), (0, -1));

            // From left
            for (int y = 0; y < treeGrid.Height; y++)
                CountVisibleTrees((0, y), (1, 0));

            // From right
            for (int y = 0; y < treeGrid.Height; y++)
                CountVisibleTrees((treeGrid.Width - 1, y), (-1, 0));

            //treeGrid.PrintToConsole();

            return visibleDict.Count;
        }
        
        int CountVisible2((int X, int Y) pos, (int DX, int DY) dir)
        {
            int count = 0;

            char maxHeight = treeGrid[pos];

            do
            {
                pos.X += dir.DX;
                pos.Y += dir.DY;

                char c;

                if (!treeGrid.TryGetValue(pos.X, pos.Y, out c))
                {
                    return count;
                }

                count++;

                if (c >= maxHeight)
                {
                    return count;
                }

            }
            while (true);
        }

        int ScoreTree((int X, int Y) pos)
        {
            int score = 1;

            // Up
            score *= CountVisible2(pos, (0, -1));

            // Down
            score *= CountVisible2(pos, (0, 1));

            // Left
            score *= CountVisible2(pos, (-1, 0));

            // Right
            score *= CountVisible2(pos, (1, 0));

            return score;
        }

        public override long Compute2()
        {
            treeGrid = new Grid<char>().CreateDataFromRows(File.ReadLines(DataFile));

            return treeGrid.GetAll().Max(t => ScoreTree(t));
        }
    }
}
