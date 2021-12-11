using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2020
{
    public class Day12
    {
        string[] commands;

        void ReadData()
        {
            commands = File.ReadLines(@"C:\Code\AdventOfCode\Input\2020\Day12.txt").ToArray();
        }

        public long Compute()
        {
            ReadData();

            int facingDir = 90;
            int eastWest = 0;
            int northSouth = 0;

            foreach (string command in commands)
            {
                char cmd = command[0];
                int amount = int.Parse(command.Substring(1));

                switch (cmd)
                {
                    case 'N':
                        northSouth += amount;
                        break;

                    case 'S':
                        northSouth -= amount;
                        break;

                    case 'E':
                        eastWest += amount;
                        break;

                    case 'W':
                        eastWest -= amount;
                        break;

                    case 'L':
                        facingDir -= amount;
                        break;

                    case 'R':
                        facingDir += amount;
                        break;

                    case 'F':
                        switch (facingDir)
                        {
                            case 0:
                                northSouth += amount;
                                break;
                            case 90:
                                eastWest += amount;
                                break;
                            case 180:
                                northSouth -= amount;
                                break;
                            case 270:
                                eastWest -= amount;
                                break;
                        }
                        break;

                }

                if (facingDir >= 360)
                    facingDir -= 360;
                else if (facingDir < 0)
                    facingDir += 360;
            }

            return Math.Abs(northSouth) + Math.Abs(eastWest);
        }

        public long Compute2()
        {
            ReadData();

            int eastWest = 0;
            int northSouth = 0;
            int wayEastWest = 10;
            int wayNorthSouth = 1;

            foreach (string command in commands)
            {
                char cmd = command[0];
                int amount = int.Parse(command.Substring(1));

                if (cmd == 'R')
                    amount = 360 - amount;

                switch (cmd)
                {
                    case 'N':
                        wayNorthSouth += amount;
                        break;

                    case 'S':
                        wayNorthSouth -= amount;
                        break;

                    case 'E':
                        wayEastWest += amount;
                        break;

                    case 'W':
                        wayEastWest -= amount;
                        break;

                    case 'R':
                    case 'L':
                        switch (amount)
                        {
                            case 90:
                                int tmp = wayNorthSouth;
                                wayNorthSouth = wayEastWest;
                                wayEastWest = -tmp;
                                break;
                            case 180:
                                wayNorthSouth = -wayNorthSouth;
                                wayEastWest = -wayEastWest;
                                break;
                            case 270:
                                tmp = wayNorthSouth;
                                wayNorthSouth = -wayEastWest;
                                wayEastWest = tmp;
                                break;
                        }
                        break;

                    case 'F':
                        northSouth += wayNorthSouth * amount;
                        eastWest += wayEastWest * amount;
                        break;
                }
            }

            return Math.Abs(northSouth) + Math.Abs(eastWest);
        }
    }
}
