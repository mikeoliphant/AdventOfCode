namespace AdventOfCode._2015
{
    internal class Day21 : Day
    {
        List<(string Name, int Cost, int Damage, int Armor)> Weapons = null;
        List<(string Name, int Cost, int Damage, int Armor)> Armor = null;
        List<(string Name, int Cost, int Damage, int Armor)> Rings = null;

        void ReadInput()
        {
            string[] gear = File.ReadAllText(DataFile).SplitParagraphs();

            Weapons = ParseGear(gear[0]).ToList();
            Armor = ParseGear(gear[1]).ToList();
            Rings = ParseGear(gear[2]).ToList();

            Armor.Add(("None", 0, 0, 0));
            Rings.Add(("None", 0, 0, 0));
            Rings.Add(("None2", 0, 0, 0));
        }

        IEnumerable<(string Name, int Cost, int Damage, int Armor)> ParseGear(string gearStr)
        {
            var lines = gearStr.SplitLines();

            foreach (string line in lines.Skip(1))
            {
                string[] split = line.SplitWhitespace();

                yield return (split[0], int.Parse(split[1]), int.Parse(split[2]), int.Parse(split[3]));
            }
        }

        IEnumerable<(int Cost, int Damage, int Armor)> GetCombos()
        {
            foreach (var weapon in Weapons)
            {
                foreach (var armor in Armor)
                {
                    yield return (weapon.Cost + armor.Cost, weapon.Damage + armor.Damage, weapon.Armor + armor.Armor);

                    for (int ring1 = 0; ring1 < Rings.Count; ring1++)
                    {
                        for (int ring2 = ring1 + 1; ring2 < Rings.Count; ring2++)
                        {
                            yield return (weapon.Cost + armor.Cost + Rings[ring1].Cost + Rings[ring2].Cost, weapon.Damage + armor.Damage + Rings[ring1].Damage + Rings[ring2].Damage, weapon.Armor + armor.Armor + Rings[ring1].Armor + Rings[ring2].Armor);
                        }
                    }
                }
            }
        }

        public override long Compute()
        {
            ReadInput();

            int bossHP = 104;
            int bossDamage = 8;
            int bossArmor = 1;

            int myHP = 100;
            int myDamage = 0;
            int myArmor = 0;

            int minCost = int.MaxValue;

            foreach (var combo in GetCombos())
            {
                myDamage = combo.Damage;
                myArmor = combo.Armor;

                int myActualDamage = Math.Max(myDamage - bossArmor, 1);
                int bossActualDamage = Math.Max(bossDamage - myArmor, 1);

                bool isWin = Math.Ceiling((float)myHP / (float)bossActualDamage) >= Math.Ceiling((float)bossHP / (float)myActualDamage);

                if (isWin)
                {
                    if (combo.Cost < minCost)
                        minCost = combo.Cost;
                }
            }

            return minCost;
        }

        public override long Compute2()
        {
            ReadInput();

            int bossHP = 104;
            int bossDamage = 8;
            int bossArmor = 1;

            int myHP = 100;
            int myDamage = 0;
            int myArmor = 0;

            int maxCost = int.MinValue;

            foreach (var combo in GetCombos())
            {
                myDamage = combo.Damage;
                myArmor = combo.Armor;

                int myActualDamage = Math.Max(myDamage - bossArmor, 1);
                int bossActualDamage = Math.Max(bossDamage - myArmor, 1);

                bool isWin = Math.Ceiling((float)myHP / (float)bossActualDamage) >= Math.Ceiling((float)bossHP / (float)myActualDamage);

                if (!isWin)
                {
                    if (combo.Cost > maxCost)
                        maxCost = combo.Cost;
                }
            }

            return maxCost;
        }
    }
}
