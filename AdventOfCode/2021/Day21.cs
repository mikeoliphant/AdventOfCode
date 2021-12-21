using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2021
{
    internal class Day21
    {
        //int[] playerPos = new int[] { 4, 8 };
        int[] playerPos = new int[] { 9, 4 };
        int[] playerScore = new int[2];
        int dieValue = 1;
        long numRolls = 0;
        int maxScore;

        bool PlayTurn()
        {
            for (int player = 0; player < 2; player++)
            {
                for (int roll = 0; roll < 3; roll++)
                {
                    numRolls++;

                    playerPos[player] = (playerPos[player] + dieValue);

                    playerPos[player] = ((playerPos[player] - 1) % 10) + 1;

                    dieValue++;

                    if (dieValue > 100)
                        dieValue = 1;
                }

                playerScore[player] += playerPos[player];

                if (playerScore[player] >= maxScore)
                    return true;
            }

            return false;
        }

        public long Compute()
        {
            maxScore = 1000;

            while (!PlayTurn());

            long result = playerScore.Min() * numRolls;

            return result;
        }

        Dictionary<int, int> rollSumCount = new Dictionary<int, int>();

        void ComputeRollPermutations()
        {
            for (int r1 = 1; r1 < 4; r1++)
            {
                for (int r2 = 1; r2 < 4; r2++)
                {
                    for (int r3 = 1; r3 < 4; r3++)
                    {
                        int sum = r1 + r2 + r3;

                        if (!rollSumCount.ContainsKey(sum))
                        {
                            rollSumCount[sum] = 1;
                        }
                        else
                        {
                            rollSumCount[sum]++;
                        }
                    }
                }
            }
        }

        Dictionary<string, long[]> knownOutcomes = new Dictionary<string, long[]>();

        void PlayGames(int[] scores, int[] positions, long numInstances)
        {
            string hash = scores[0] + "-" + positions[0] + "," + scores[1] + "-" + positions[1];

            long[] numWinsAtStart = numWins.Clone() as long[];

            if (knownOutcomes.ContainsKey(hash))
            {
                numWins[0] += knownOutcomes[hash][0] * numInstances;
                numWins[1] += knownOutcomes[hash][1] * numInstances;

                return;
            }

            foreach (var rolls1 in rollSumCount)
            {
                bool playerOneWon = false;

                foreach (var rolls2 in rollSumCount)
                {
                    int[] localScores = scores.Clone() as int[];
                    int[] localPositions = positions.Clone() as int[];

                    bool haveWinner = false;

                    for (int player = 0; player < 2; player++)
                    {
                        localPositions[player] = localPositions[player] + ((player == 0) ? rolls1.Key : rolls2.Key);
                        localPositions[player] = ((localPositions[player] - 1) % 10) + 1;

                        localScores[player] += localPositions[player];

                        if (localScores[player] >= maxScore)
                        {
                            numWins[player] += ((player == 0) ? (rolls1.Value * rolls2.Value) : (rolls1.Value * rolls2.Value)) * numInstances;

                            haveWinner = true;

                            if (player == 0)
                                playerOneWon = true;

                            break;
                        }
                    }

                    if (!haveWinner)
                    {
                        PlayGames(localScores, localPositions, numInstances * rolls1.Value * rolls2.Value);
                    }

                    if (playerOneWon)   // If player one won, he wins for all player2 combos
                        break;
                }
            }

            knownOutcomes[hash] = new long[]
            {
                (numWins[0] - numWinsAtStart[0]) / numInstances,
                (numWins[1] - numWinsAtStart[1]) / numInstances
            };
        }

        long[] numWins = new long[2];

        public long Compute2()
        {
            maxScore = 21;

            ComputeRollPermutations();

            PlayGames(playerScore, playerPos, 1);

            long result = numWins.Max();

            return result;
        }
    }
}
