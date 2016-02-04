using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RyzePro.Modes
{
    class Harass
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        public static int PassiveStack = Starting.StackPassive;
        public static AIHeroClient Target;
        public static void Do()
        {
            if (!TargetSelector.SelectedTarget.IsValidTarget(2500))
            {
                Target = TargetSelector.GetTarget(2500, DamageType.Magical);
            }
            else if (TargetSelector.SelectedTarget.IsValidTarget(2500))
            {
                Target = TargetSelector.SelectedTarget;
            }

            if (Ryze.ManaPercent < Checker.HarassMana)
            {
                return;
            }

            if (Checker.HarassUseQ && Spells.Q.IsReady() && Target.IsValidTarget(Spells.Q.Range))
            {
                var pred = Spells.Q.GetPrediction(Target);
                if (pred.HitChance >= HitChance.High && pred.CollisionObjects.Count() == 0)
                {
                    Spells.Q.Cast(Target);
                }
            }

            if (Checker.HarassUseW && Spells.W.IsReady() && Target.IsValidTarget(Spells.W.Range))
            {
                Spells.W.Cast(Target);
            }

            if (Checker.HarassUseE && Spells.E.IsReady() && Target.IsValidTarget(Spells.E.Range))
            {
                Spells.E.Cast(Target);
            }
        }
    }
}
