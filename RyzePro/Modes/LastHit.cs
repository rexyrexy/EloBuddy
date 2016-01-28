using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class LastHit
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static int PassiveStack = Starting.StackPassive;
        public static void Do()
        {
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => !x.IsDead && Spells.Q.IsInRange(x));
            if (minions == null)
            {
                return;
            }

            if (Spells.Q.IsReady() && Checker.LastHitUseQ)
            {
                if (Prediction.Health.GetPrediction(minions,(int)0.25)
                    <= Ryze.GetSpellDamage(minions, SpellSlot.Q))
                {
                    Spells.Q.Cast(minions);
                }
            }
        }
    }
}
