using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class LaneClear
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static int PassiveStack = Starting.StackPassive;
        public static void Do()
        {
            if (Ryze.ManaPercent < Checker.LaneClearMana)
            {
                return;
            }

            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => !x.IsDead && Spells.W.IsInRange(x));
            if (minions == null)
            {
                return;
            }

            if (Checker.LaneClearUseW && Spells.W.IsReady())
            {
                Spells.W.Cast(minions);
            }

            if (Checker.LaneClearUseQ && Spells.Q.IsReady())
            {
                if (Prediction.Health.GetPrediction(minions, (int)0.25)
                    <= Ryze.GetSpellDamage(minions, SpellSlot.Q))
                {
                    Spells.Q.Cast(minions);
                }
            }

            if (Checker.LaneClearUseE && Spells.E.IsReady())
            {
                Spells.E.Cast(minions);
            }
        }
    }
}
