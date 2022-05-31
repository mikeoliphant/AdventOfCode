namespace AdventOfCode._2015
{
    internal class Day15 : Day
    {
        int[,] ingredients = null;
        string[] ingredientNames = null;
        int numIngredients;

        IEnumerable<int[]> GetAllPermutations(int[] measures, int pos, int left)
        {
            if (pos == (measures.Length - 1))
            {
                measures[pos] = left;

                yield return measures;
            }
            else
            {
                for (int toUse = 0; toUse < left; toUse++)
                {
                    measures[pos] = toUse;

                    foreach (var perm in GetAllPermutations(measures, pos + 1, left - toUse))
                    {
                        yield return perm;
                    }
                }
            }
        }

        int GetScore(int[] measures)
        {
            int score = 1;

            for (int prop = 0; prop < 4; prop++)
            {
                int measureScore = 0;

                for (int measure = 0; measure < measures.Length; measure++)
                {
                    measureScore += measures[measure] * ingredients[measure, prop];
                }

                score *= Math.Max(measureScore, 0);
            }

            return score;
        }

        int GetCalories(int[] measures)
        {
            int calories = 0;

            for (int measure = 0; measure < measures.Length; measure++)
            {
                calories += measures[measure] * ingredients[measure, 4];
            }

            return calories;
        }

        void ReadInput()
        {
            var ingredientData = File.ReadLines(DataFile);

            numIngredients = ingredientData.Count();

            ingredients = new int[numIngredients, 5];
            ingredientNames = new string[numIngredients];

            int index = 0;

            foreach (string ingredient in ingredientData)
            {
                string[] split = ingredient.Split(": ");

                ingredientNames[index] = split[0];

                string[] props = split[1].Split(", ");

                for (int i = 0; i < 5; i++)
                {
                    string[] nameVal = props[i].Split(' ');

                    ingredients[index, i] = int.Parse(nameVal[1]);
                }

                index++;
            }
        }

        public override long Compute()
        {
            ReadInput();

            IEnumerable<int[]> perms = GetAllPermutations(new int[numIngredients], 0, 100);

            return perms.Max(p => GetScore(p));
        }

        public override long Compute2()
        {
            ReadInput();

            IEnumerable<int[]> perms = GetAllPermutations(new int[numIngredients], 0, 100).Where(p => GetCalories(p) == 500);

            return perms.Max(p => GetScore(p));
        }
    }
}
