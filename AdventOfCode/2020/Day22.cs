namespace AdventOfCode._2020
{
    internal class Day22
    {
        List<int>[] playerDecks;

        void ReadInput()
        {
            playerDecks = (from deck in File.ReadAllText(@"C:\Code\AdventOfCode\Input\2020\Day22.txt").SplitParagraphs() select new List<int>(deck.SplitLines().Skip(1).ToInts())).ToArray();
        }

        public long Compute()
        {
            ReadInput();

            do
            {
                if (playerDecks[0][0] > playerDecks[1][0])
                {
                    playerDecks[0].Add(playerDecks[0][0]);
                    playerDecks[0].Add(playerDecks[1][0]);
                }
                else
                {
                    playerDecks[1].Add(playerDecks[1][0]);
                    playerDecks[1].Add(playerDecks[0][0]);
                }

                playerDecks[0].RemoveAt(0);
                playerDecks[1].RemoveAt(0);
            }
            while ((playerDecks[0].Count > 0) && (playerDecks[1].Count > 0));

            List<int> winningDeck = (playerDecks[0].Count > 0) ? playerDecks[0] : playerDecks[1];

            long score = 0;

            for (int i = 0; i < winningDeck.Count; i++)
            {
                score += winningDeck[i] * (winningDeck.Count - i);
            }

            return score;
        }

        Dictionary<string, int> previousWinners = new Dictionary<string, int>();

        int PlayGame(List<int>[] decks)
        {
            string startDeckHash = String.Join('-', decks[0]) + "|" + String.Join('-', decks[1]);

            if (previousWinners.ContainsKey(startDeckHash))
            {
                return previousWinners[startDeckHash];
            }

            Dictionary<string, int> previousDecksThisGame = new Dictionary<string, int>();

            do
            {
                int top0 = decks[0][0];
                int top1 = decks[1][0];

                int winner;

                string deckHash = String.Join('-', decks[0]) + "|" + String.Join('-', decks[1]);

                decks[0].RemoveAt(0);
                decks[1].RemoveAt(0);

                if (previousDecksThisGame.ContainsKey(deckHash))
                {
                    return 0;
                }
                else
                {
                    previousDecksThisGame[deckHash] = 0;

                    if ((top0 <= decks[0].Count) && (top1 <= decks[1].Count))
                    {
                        winner = PlayGame(new List<int>[] { decks[0].GetRange(0, top0), decks[1].GetRange(0, top1) });
                    }
                    else
                    {
                        winner = (top0 > top1) ? 0 : 1;
                    }
                }

                if (winner == 0)
                {
                    decks[0].Add(top0);
                    decks[0].Add(top1);
                }
                else
                {
                    decks[1].Add(top1);
                    decks[1].Add(top0);
                }
            }
            while ((decks[0].Count > 0) && (decks[1].Count > 0));

            int gameWinner = (decks[0].Count > 0) ? 0 : 1;

            previousWinners[startDeckHash] = gameWinner;

            return gameWinner;
        }

        public long Compute2()
        {
            ReadInput();

            List<int> winningDeck = playerDecks[PlayGame(playerDecks)];

            long score = 0;

            for (int i = 0; i < winningDeck.Count; i++)
            {
                score += winningDeck[i] * (winningDeck.Count - i);
            }

            return score;
        }
    }
}
