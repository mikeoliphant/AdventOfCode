namespace AdventOfCode._2020
{
    public class Bag
    {
        public string Adjective { get; set; }
        public string Color { get; set; }
        public List<Bag> ContainedIn { get; set; }
        public List<KeyValuePair<int, Bag>> CanContain { get; set; }

        public Bag()
        {
            ContainedIn = new List<Bag>();
            CanContain = new List<KeyValuePair<int, Bag>>();            
        }

        public override string ToString()
        {
            return Adjective + " " + Color;
        }
    }

    public class Day7
    {
        Dictionary<string, Bag> bags = new Dictionary<string, Bag>();

        Bag GetBag(string adjective, string color)
        {
            string key = adjective + " " + color;

            if (!bags.ContainsKey(key))
            {
                bags[key] = new Bag { Adjective = adjective, Color = color };
            }

            return bags[key];
        }

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day7.txt"))
            {
                string[] lrStr = line.Split("contain");

                string[] bagStr = lrStr[0].Split(' ');

                Bag bag = GetBag(bagStr[0], bagStr[1]);

                string[] containStrs = lrStr[1].Split(',');

                foreach (string containStr in containStrs)
                {
                    string[] containsBagStr = containStr.Trim().Split(' ');

                    if (containsBagStr[0] == "no")
                        continue;

                    Bag containsBag = GetBag(containsBagStr[1], containsBagStr[2]);

                    bag.CanContain.Add(new KeyValuePair<int, Bag>(int.Parse(containsBagStr[0]), containsBag));

                    containsBag.ContainedIn.Add(bag);
                }
            }
        }

        public long GetNumPaths(Bag startBag, Bag destBag)
        {
            long numPaths = 0;

            foreach (var contains in startBag.CanContain)
            {
                if (contains.Value == destBag)
                {
                    numPaths++;
                }
                else
                {
                    numPaths += GetNumPaths(contains.Value, destBag);
                }
            }

            return numPaths;
        }

        public long GetTotalBags(Bag startBag)
        {
            long totBags = 0;

            foreach (var contains in startBag.CanContain)
            {
                totBags += contains.Key * GetTotalBags(contains.Value);
            }

            return totBags + 1;
        }

        public long Compute()
        {
            ReadInput();

            Bag shinyGold = GetBag("shiny", "gold");

            long numBags = 0;

            foreach (Bag bag in bags.Values)
            {
                if (GetNumPaths(bag, shinyGold) > 0)
                    numBags++;
            }

            return numBags;
        }

        public long Compute2()
        {
            ReadInput();

            Bag shinyGold = GetBag("shiny", "gold");

            return GetTotalBags(shinyGold) - 1;
        }
    }
}
