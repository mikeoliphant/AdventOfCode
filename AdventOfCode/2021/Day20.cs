namespace AdventOfCode._2021
{
    internal class Day20
    {
        string algorithm;
        Grid<char> image;

        void ReadInput()
        {
            string[] data = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2021\Day20.txt").SplitParagraphs();

            algorithm = String.Join("", data[0].SplitLines());

            image = new Grid<char>().CreateDataFromRows(data[1].SplitLines());

            //image.PrintToConsole();
        }

        char invalidValue = '.';

        void Cycle()
        {
            Grid<char> expandedImage = new Grid<char>(image.Width + 4, image.Height + 4);
            expandedImage.Fill(invalidValue);

            Grid<char>.Copy(image, expandedImage, 2, 2);

            image = expandedImage;

            image.InvalidValue = invalidValue;

            invalidValue = (invalidValue == '.') ? '#' : '.';

            Grid<char> newImage = new Grid<char>(image.Width, image.Height);

            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    string binaryString = "";

                    foreach (char c in image.GetWindowValues(x, y, 1, includeSelf: true))
                    {
                        binaryString += (c == '#') ? '1' : '0';
                    }

                    int value = Convert.ToInt32(binaryString, 2);

                    newImage[x, y] = algorithm[value];
                }
            }

            image = newImage;
        }

        public long Compute()
        {
            ReadInput();

            for (int turn = 0; turn < 50; turn++)
            {
                Cycle();
            }

            image.PrintToConsole();

            int numLit = (from c in image.GetAllValues() where c == '#' select c).Count();

            return numLit;
        }
    }
}
