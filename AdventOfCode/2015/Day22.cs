namespace AdventOfCode._2015
{
    internal class Day22 : Day
    {
        int bossHP = 71;
        int bossDamage = 10;

        IEnumerable<KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>> GetNeighbors((int PlayerHP, int PlayerMana, int BossHP, int ShieldTurns, int PoisonTurns, int RechargeTurns) state)
        {
            state.PlayerHP--;

            if (state.PlayerHP > 0)
            {
                if (state.PoisonTurns >= 2)
                {
                    state.BossHP -= 6;
                }
                else if (state.PoisonTurns == 1)
                {
                    state.BossHP -= 3;
                }

                if (state.ShieldTurns >= 2)
                {
                    state.PlayerHP -= Math.Max(bossDamage - 7, 1);
                }
                else
                {
                    state.PlayerHP -= bossDamage;
                }

                if (state.RechargeTurns > 0)
                {
                    state.PlayerMana += 101;
                }

                int startMana = state.PlayerMana;

                if (state.RechargeTurns >= 2)
                {
                    state.PlayerMana += 101;
                }

                if ((state.ShieldTurns < 2) && (startMana >= 113))
                {
                    yield return new KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>(
                        (state.PlayerHP + bossDamage - (Math.Max(bossDamage - 7, 1)), state.PlayerMana - 113, state.BossHP, 5, Math.Max(state.PoisonTurns - 2, 0), Math.Max(state.RechargeTurns - 2, 0)), 113);
                }

                if ((state.PoisonTurns < 2) && (startMana >= 173))
                {
                    yield return new KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>(
                        (state.PlayerHP, state.PlayerMana - 173, state.BossHP - 3, Math.Max(state.ShieldTurns - 2, 0), 5, Math.Max(state.RechargeTurns - 2, 0)), 173);
                }

                if ((state.RechargeTurns < 2) && (startMana >= 229))
                {
                    yield return new KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>(
                        (state.PlayerHP, state.PlayerMana - 229 + 101, state.BossHP, Math.Max(state.ShieldTurns - 2, 0), Math.Max(state.PoisonTurns - 2, 0), 4), 229);
                }

                // Magic Missile
                if (startMana >= 53)
                {
                    yield return new KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>(
                        (state.PlayerHP, state.PlayerMana - 53, state.BossHP - 4, Math.Max(state.ShieldTurns - 2, 0), Math.Max(state.PoisonTurns - 2, 0), Math.Max(state.RechargeTurns - 2, 0)), 53);
                }

                // Drain
                if (startMana >= 73)
                {
                    yield return new KeyValuePair<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns), float>(
                        (state.PlayerHP + 2, state.PlayerMana - 73, state.BossHP - 2, Math.Max(state.ShieldTurns - 2, 0), Math.Max(state.PoisonTurns - 2, 0), Math.Max(state.RechargeTurns - 2, 0)), 73);
                }
            }
        }

        public override long Compute()
        {
            DijkstraSearch<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns)> search = new DijkstraSearch<(int PlayerHP, int PlayerMana, int BossHP, int ShieldTurns, int PoisonTurns, int RechargeTurns)>(GetNeighbors);

            int playerHP = 50;
            int playerMana = 500;

            List<(int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns)> path;
            float cost;

            if (search.GetShortestPath((playerHP, playerMana, bossHP, 0, 0, 0), delegate ((int PlayerHP, int PlayerMana, int BossHP, int shieldTurns, int poisonTurns, int RechargeTurns) state)
            {
                return state.BossHP <= 0;
            }, out path, out cost))
            {
                return (long)cost;
            }

            throw new InvalidOperationException();
        }
    }
}
