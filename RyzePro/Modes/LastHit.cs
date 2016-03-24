using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class LastHit
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static void Do()
        {
            foreach (var minions in EntityManager.MinionsAndMonsters.Get(EntityManager.MinionsAndMonsters.EntityType.Both,EntityManager.UnitTeam.Enemy,Player.Instance.Position,Spells.Q.Range))
            {
                if (Spells.Q.IsReady() && Checker.LastHitUseQ)
                {
                    if (Prediction.Health.GetPrediction(minions, (int)0.25)
                        <= Ryze.GetSpellDamage(minions, SpellSlot.Q))
                    {
                        Spells.Q.Cast(minions);
                    }
                }   
            }
        }
    }
}
