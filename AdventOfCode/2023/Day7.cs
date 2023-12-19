namespace AdventOfCode._2023
{
    internal class Day7 : Day
    {
        static Dictionary<char, int> cardRank = new();

        int HandScore(string hand)
        {
            Dictionary<char, int> ofAKind = new Dictionary<char, int>();

            foreach (char c in hand)
            {
                if (!ofAKind.ContainsKey(c))
                    ofAKind[c] = 1;
                else
                    ofAKind[c]++;
            }

            var sorted = ofAKind.Values.Order().ToArray();

            int handRank = 0;

            if (sorted.Contains(5))
                handRank = 6;
            else if (sorted.Contains(4))
                handRank = 5;
            else if (sorted.Contains(3))
                handRank = sorted.Contains(2) ? 4 : 3;
            else if (sorted.Count(2) == 2)
                handRank = 2;
            else if (sorted.Contains(2))
                handRank = 1;

            return handRank;
        }

        class CardComparer : IComparer<string>
        {
            public int Compare(string a, string b)
            {
                for (int pos = 0; pos < a.Length; pos++)
                {
                    int diff = cardRank[a[pos]] - cardRank[b[pos]];

                    if (diff != 0)
                        return diff;
                }

                return 0;
            }
        }

        void Init()
        {
            int rank = 0;

            for (char c = '2'; c <= '9'; c++)
            {
                cardRank[c] = rank++;
            }

            cardRank['T'] = rank++;
            cardRank['J'] = rank++;
            cardRank['Q'] = rank++;
            cardRank['K'] = rank++;
            cardRank['A'] = rank++;
        }

        public override long Compute()
        {
            Init();

            Dictionary<string, int> handScore = new();
            Dictionary<string, int> handBid = new();

            foreach (string hand in File.ReadLines(DataFile))
            {
                string[] split = hand.Split(' ');

                handScore[split[0]] = HandScore(split[0]);

                handBid[split[0]] = int.Parse(split[1]);
            }

            var sorted = handScore.OrderBy(h => h.Value).ThenBy(h => h.Key, new CardComparer());

            long winnings = 0;

            int rank = 1;

            foreach (var hand in sorted)
            {
                winnings += handBid[hand.Key] * rank;

                rank++;
            }

            return winnings;
        }

        int HandScoreJokersWild(string hand)
        {
            int maxRank = 0;

            foreach (char j in cardRank.Keys)
            {
                if (j == 'J')
                    continue;

                Dictionary<char, int> ofAKind = new Dictionary<char, int>();

                foreach (char c in hand)
                {
                    char cj = c;

                    if (cj == 'J')
                        cj = j;

                    if (!ofAKind.ContainsKey(cj))
                        ofAKind[cj] = 1;
                    else
                        ofAKind[cj]++;
                }

                var sorted = ofAKind.Values.Order().ToArray();

                int handRank = 0;

                if (sorted.Contains(5))
                    handRank = 6;
                else if (sorted.Contains(4))
                    handRank = 5;
                else if (sorted.Contains(3))
                    handRank = sorted.Contains(2) ? 4 : 3;
                else if (sorted.Count(2) == 2)
                    handRank = 2;
                else if (sorted.Contains(2))
                    handRank = 1;

                maxRank = Math.Max(handRank, maxRank);
            }

            return maxRank;
        }

        public override long Compute2()
        {
            Init();

            cardRank['J'] = -1;

            Dictionary<string, int> handScore = new();
            Dictionary<string, int> handBid = new();

            foreach (string hand in File.ReadLines(DataFile))
            {
                string[] split = hand.Split(' ');

                handScore[split[0]] = HandScoreJokersWild(split[0]);

                handBid[split[0]] = int.Parse(split[1]);
            }

            var sorted = handScore.OrderBy(h => h.Value).ThenBy(h => h.Key, new CardComparer());

            long winnings = 0;

            int rank = 1;

            foreach (var hand in sorted)
            {
                winnings += handBid[hand.Key] * rank;

                rank++;
            }

            return winnings;
        }
    }
}
