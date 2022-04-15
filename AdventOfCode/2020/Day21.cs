namespace AdventOfCode._2020
{
    internal class Day21
    {
        List<KeyValuePair<List<string>, List<string>>> ingredients = new List<KeyValuePair<List<string>, List<string>>>();
        Dictionary<string, List<string>> matches = new Dictionary<string, List<string>>();
        Dictionary<string, string> toEnglish = new Dictionary<string, string>();
        Dictionary<string, string> fromEnglish = new Dictionary<string, string>();

        void ReadInput()
        {
            foreach (string line in File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day21.txt"))
            {
                string[] lr = line.Split(" (");

                string contains = lr[1].Substring(9, lr[1].Length - 10);

                ingredients.Add(new KeyValuePair<List<string>, List<string>>(new List<string>(lr[0].Split(' ')), new List<string>(contains.Split(", "))));
            }
        }

        bool DoElimination()
        {
            matches.Clear();

            foreach (var ingredient1 in ingredients)
            {
                foreach (var ingredient2 in ingredients)
                {
                    if (!ingredient1.Equals(ingredient2))
                    {
                        foreach (string allergen in ingredient1.Value)
                        {
                            if (ingredient2.Value.Contains(allergen))
                            {
                                List<string> possibleMatches = new List<string>();

                                foreach (string foreign in ingredient1.Key)
                                {
                                    if (ingredient2.Key.Contains(foreign))
                                    {
                                        possibleMatches.Add(foreign);
                                    }
                                }

                                if (!matches.ContainsKey(allergen))
                                {
                                    matches[allergen] = possibleMatches;
                                }
                                else
                                {
                                    List<string> newMatches = new List<string>();

                                    foreach (string match in matches[allergen])
                                    {
                                        if (possibleMatches.Contains(match))
                                            newMatches.Add(match);
                                    }

                                    matches[allergen] = newMatches;
                                }
                            }
                        }
                    }
                }
            }

            bool eliminated = false;

            foreach (var matches in matches)
            {
                if (matches.Value.Count == 1)
                {
                    eliminated = true;

                    string foreign = matches.Value[0];

                    fromEnglish[matches.Key] = foreign;
                    toEnglish[foreign] = matches.Key;

                    foreach (var ingredient in ingredients)
                    {
                        ingredient.Key.Remove(foreign);
                        ingredient.Value.Remove(matches.Key);
                    }
                }
            }

            foreach (var ingredient in ingredients)
            {
                if (ingredient.Key.Count == 1)
                {
                    if (ingredient.Value.Count == 1)
                    {
                        eliminated = true;

                        string english = ingredient.Value[0];
                        string foreign = ingredient.Key[0];

                        fromEnglish[english] = foreign;
                        toEnglish[foreign] = english;

                        foreach (var ingredient2 in ingredients)
                        {
                            ingredient2.Key.Remove(foreign);
                            ingredient2.Value.Remove(english);
                        }
                    }
                }
            }

            return eliminated;
        }

        public long Compute()
        {
            ReadInput();

            while (DoElimination()) ;

            int numMissing = 0;

            foreach (var ingredient in ingredients)
            {
                numMissing += ingredient.Key.Count;
            }

            return numMissing;
        }

        public long Compute2()
        {
            ReadInput();

            while (DoElimination()) ;

            string dangerous = String.Join(',', from allergen in fromEnglish.Keys orderby allergen select fromEnglish[allergen]);

            return 0;
        }
    }
}
