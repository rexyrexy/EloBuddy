using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class LaneClear
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static void Do()
        {
            if (Ryze.ManaPercent < Checker.LaneClearMana)
            {
                return;
            }

            foreach (var minions in EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy,Player.Instance.Position,Spells.Q.Range))
            {
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

            if (Checker.LaneClearUseR && Spells.R.IsReady())
            {
                Spells.R.Cast();
            }
            }
        }
    }
}
