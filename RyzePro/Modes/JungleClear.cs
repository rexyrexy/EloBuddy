using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class JungleClear
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static int PassiveStack = Starting.StackPassive;
        public static void Do()
        {
            if (Ryze.ManaPercent < Checker.JungleClearMana)
            {
                return;
            }

            foreach (var minions in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (minions == null)
                {
                    return;
                }

                if (Checker.JungleClearUseQ && Spells.Q.IsReady() && minions.IsValidTarget(Spells.Q.Range))
                {
                    Spells.Q.Cast(minions);
                }

                if (Checker.JungleClearUseW && Spells.W.IsReady() && minions.IsValidTarget(Spells.W.Range))
                {
                    Spells.W.Cast(minions);
                }

                if (Checker.JungleClearUseE && Spells.E.IsReady() && minions.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(minions);
                }
            }
        }
    }
}
