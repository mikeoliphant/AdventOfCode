namespace AdventOfCode._2018
{
    internal class Day24
    {
        class ArmyGroup
        {
            public string Name { get; set; }
            public int NumUnits { get; set; }
            public int HitPoints { get; set; }
            public string[] ImmuneTo { get; set; } = new string[0];
            public string[] WeakTo { get; set; } = new string[0];
            public string DamageType { get; set; }
            public int DamageAmount { get; set; }
            public int Initiative { get; set; }

            public int Power
            {
                get { return NumUnits * DamageAmount; }
            }

            public int DamageTo(ArmyGroup group)
            {
                if (group.ImmuneTo.Contains(DamageType))
                    return 0;

                if (group.WeakTo.Contains(DamageType))
                {
                    return Power * 2;
                }

                return Power;
            }

            public void Attack(ArmyGroup group)
            {
                int damage = DamageTo(group);

                int deadUnits = damage / group.HitPoints;

                if (deadUnits > group.NumUnits)
                    deadUnits = group.NumUnits;

                group.NumUnits -= deadUnits;

                //Console.WriteLine(Name + " attacks " + group.Name + " killing " + deadUnits + " units");
            }

            public override string ToString()
            {
                return Name + ": " + NumUnits + " units each with " + HitPoints + " hit points (immune to " + String.Join(", ", ImmuneTo) + "; weak to " + String.Join(", ", WeakTo) + ") with an attack that does " + DamageAmount + " " + DamageType + " damage at initiative " + Initiative;
            }
        }

        List<ArmyGroup>[] armies;
        int immuneBoost = 0;

        void ReadInput()
        {
            int pos = 0;

            armies = new List<ArmyGroup>[2];

            foreach (string armyStr in ParseHelpers.SplitParagraphs(File.ReadAllText(@"C:\Code\AdventOfCode\Input\2018\Day24.txt")))
            {
                armies[pos] = new List<ArmyGroup>();

                string name = Regex.Match(armyStr, "^(.*):").Groups[1].Value;

                foreach (string group in armyStr.SplitLines().Skip(1))
                {
                    Match match = Regex.Match(group, @"(.*) units each with (.*) hit points (\((.*)\) )?with an attack that does (.*) (.*) damage at initiative (.*)");

                    ArmyGroup armyGroup = new ArmyGroup();

                    armyGroup.Name = name + " " + (armies[pos].Count + 1);
                    armyGroup.NumUnits = int.Parse(match.Groups[1].Value);
                    armyGroup.HitPoints = int.Parse(match.Groups[2].Value);

                    if (match.Groups[4].Captures.Count != 0)
                    {
                        string[] split = match.Groups[4].Value.Split("; ");

                        foreach (string s in split)
                        {
                            string[] split2 = s.Split(" to ");

                            string[] to = split2[1].Split(", ");

                            if (split2[0] == "immune")
                            {
                                armyGroup.ImmuneTo = to;
                            }
                            else
                            {
                                armyGroup.WeakTo = to;
                            }
                        }
                    }

                    armyGroup.DamageAmount = int.Parse(match.Groups[5].Value);

                    if (name.StartsWith("Immune"))
                    {
                        armyGroup.DamageAmount += immuneBoost;
                    }

                    armyGroup.DamageType = match.Groups[6].Value;
                    armyGroup.Initiative = int.Parse(match.Groups[7].Value);

                    armies[pos].Add(armyGroup);
                }

                pos++;
            }
        }

        void DoAttack()
        {
            Dictionary<ArmyGroup, ArmyGroup> toAttack = new Dictionary<ArmyGroup, ArmyGroup>();

            for (int pos = 0; pos < 2; pos++)
            {
                List<ArmyGroup> otherArmy = new List<ArmyGroup>(armies[1 - pos]);

                foreach (ArmyGroup armyGroup in armies[pos].OrderByDescending(g => g.Power).ThenByDescending(g => g.Initiative))
                {
                    ArmyGroup attack = otherArmy.OrderByDescending(g => armyGroup.DamageTo(g)).ThenByDescending(g => g.Power).ThenByDescending(g => g.Initiative).First();

                    toAttack[armyGroup] = attack;

                    otherArmy.Remove(attack);

                    if (otherArmy.Count == 0)
                        break;
                }
            }

            foreach (ArmyGroup group in toAttack.Keys.OrderByDescending(g => g.Initiative))
            {
                group.Attack(toAttack[group]);
            }

            //Console.WriteLine();
        }

        void RunCombat()
        {
            while ((armies[0].Count > 0) && (armies[1].Count > 0))
            {
                DoAttack();

                foreach (var army in armies)
                {
                    army.RemoveAll(g => g.NumUnits == 0);
                }
            }
        }

        public long Compute()
        {
            ReadInput();

            RunCombat();

            var winner = (armies[0].Count == 0) ? armies[1] : armies[0];

            return winner.Sum(g => g.NumUnits);
        }

        public long Compute2()
        {
            immuneBoost = 60;

            do
            {
                ReadInput();

                RunCombat();

                if (armies[1].Count == 0)
                    break;

                Console.WriteLine("Boost: " + immuneBoost + " Infection power: " + armies[1].Sum(g => g.NumUnits));

                immuneBoost += 1;
            }
            while (true);

            return armies[0].Sum(g => g.NumUnits);
        }
    }
}
