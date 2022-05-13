namespace AdventOfCode._2016
{
    internal class Day21
    {
        string RunInstructions(IEnumerable<string> instructions, string startStr)
        {
            CircularList<char> password = new CircularList<char>(startStr);

            int length = password.Count;

            //Console.WriteLine(new String(password.ToArray()));

            foreach (string inst in instructions)
            {
                Match match = Regex.Match(inst, "rotate (.*) (.*) step.*");

                if (match.Success)
                {
                    int amount = int.Parse(match.Groups[2].Value);

                    if (match.Groups[1].Value == "left")
                    {
                        amount = -amount;
                    }

                    password.Rotate(amount);
                }
                else
                {
                    match = Regex.Match(inst, "rotate based on position of letter (.*)");

                    if (match.Success)
                    {
                        int offset = password.IndexOf(match.Groups[1].Value[0]);

                        if (offset >= 4)
                            offset++;

                        password.Rotate(offset + 1);
                    }
                    else
                    {
                        match = Regex.Match(inst, "reverse positions (.*) through (.*)");

                        if (match.Success)
                        {
                            int pos1 = int.Parse(match.Groups[1].Value);
                            int pos2 = int.Parse(match.Groups[2].Value);

                            int size = (pos2 - pos1) + 1;

                            for (int pos = 0; pos < (size / 2); pos++)
                            {
                                int p1 = pos1 + pos;
                                int p2 = pos2 - pos;

                                char tmp = password[p1];
                                password[p1] = password[p2];
                                password[p2] = tmp;
                            }
                        }
                        else
                        {
                            match = Regex.Match(inst, "swap position (.*) with position (.*)");

                            if (match.Success)
                            {
                                var letter1 = password.Position(int.Parse(match.Groups[1].Value));
                                var letter2 = password.Position(int.Parse(match.Groups[2].Value));

                                char tmp = letter1.Value;
                                letter1.Value = letter2.Value;
                                letter2.Value = tmp;
                            }
                            else
                            {
                                match = Regex.Match(inst, "move position (.*) to position (.*)");

                                if (match.Success)
                                {
                                    var from = password.Position(int.Parse(match.Groups[1].Value));
                                    int toPos = int.Parse(match.Groups[2].Value);

                                    password.Remove(from);
                                    password.InsertAt(toPos, from);
                                }
                                else
                                {
                                    match = Regex.Match(inst, "swap letter (.*) with letter (.*)");

                                    if (match.Success)
                                    {
                                        var letter1 = password.Find(match.Groups[1].Value[0]);
                                        var letter2 = password.Find(match.Groups[2].Value[0]);

                                        char tmp = letter1.Value;
                                        letter1.Value = letter2.Value;
                                        letter2.Value = tmp;
                                    }
                                    else
                                    {
                                        throw new InvalidOperationException();
                                    }
                                }
                            }
                        }
                    }
                }

                //Console.WriteLine(inst);
                //Console.WriteLine(new String(password.ToArray()));
                //Console.WriteLine();
            }

            return new string(password.ToArray());
        }

        public long Compute()
        {
            string[] instructions = File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day21.txt").ToArray();

            string passwordStr = RunInstructions(instructions, "abcdefgh");


            return 0;
        }

        public long Compute2()
        {
            string[] instructions = File.ReadLines(@"C:\Code\AdventOfCode\Input\2016\Day21.txt").ToArray();

            foreach (var permutation in PermutationHelper<char>.GetAllPermutations("abcdefgh"))
            {
                string scrambled = RunInstructions(instructions, new string(permutation));

                if (scrambled == "fbgdceah")
                {
                    string passwordStr = new string(permutation);
                }
            }

            return 0;
        }
    }
}
