namespace AdventOfCode._2020
{
    public class Day4
    {
        string[] requiredIDs = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" }; //, "cid" };

        Func<string, bool>[] validateActions =
        {
            delegate(string id) // byr
            {
                int year = 0;

                if (int.TryParse(id, out year))
                {
                    return ((year >= 1920) && (year <= 2002));
                }

                return false;
            },
            delegate(string id) // iyr
            {
                int year = 0;

                if (int.TryParse(id, out year))
                {
                    return ((year >= 2010) && (year <= 2020));
                }

                return false;
            },
            delegate(string id) // eyr
            {
                int year = 0;

                if (int.TryParse(id, out year))
                {
                    return ((year >= 2020) && (year <= 2030));
                }

                return false;
            },
            delegate(string id) // hgt
            {
                int height;

                if (int.TryParse(id.Substring(0, id.Length - 2), out height))
                {
                    if (id.EndsWith("cm"))
                    {
                        return (height >= 150) && (height <= 193);
                    }

                    if (id.EndsWith("in"))
                    {
                        return (height >= 59) && (height <= 76);
                    }
                }

                return false;
            },
            delegate(string id) // hcl
            {
                return Regex.IsMatch(id, @"^#[0-9a-f]{6}$");
            },
            delegate(string id) // ecl
            {
                return (id == "amb") || (id == "blu") || (id == "brn") || (id == "gry") || (id == "grn") || (id == "hzl") || (id == "oth");
            },
            delegate(string id) // pid
            {
                return Regex.IsMatch(id, @"^\d{9}$");
            }
        };

        public long Compute()
        {
            string[] passports = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day4.txt").Split("\n\n");

            int numValid = 0;

            foreach (string passport in passports)
            {
                string[] ids = Regex.Split(passport, "[^a-zA-Z0-9]+");

                int matchedIDs = 0;

                foreach (string requiredID in requiredIDs)
                {
                    for (int pos = 0; pos < ids.Length; pos += 2)
                    {
                        if (ids[pos] == requiredID)
                        {
                            matchedIDs++;

                            break;
                        }
                    }
                }

                if (matchedIDs == requiredIDs.Length)
                    numValid++;
            }

            return numValid;
        }

        public long Compute2()
        {
            string[] passports = File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day4.txt").Split("\n\n");

            int numValid = 0;

            foreach (string passport in passports)
            {
                string[] ids = Regex.Split(passport, "[^a-zA-Z0-9#]+");

                int matchedIDs = 0;

                int idPos = 0;

                foreach (string requiredID in requiredIDs)
                {
                    for (int pos = 0; pos < ids.Length; pos += 2)
                    {
                        if (ids[pos] == requiredID)
                        {
                            if (validateActions[idPos](ids[pos + 1]))
                                matchedIDs++;

                            break;
                        }
                    }

                    idPos++;
                }

                if (matchedIDs == requiredIDs.Length)
                    numValid++;
            }

            return numValid;
        }
    }
}
