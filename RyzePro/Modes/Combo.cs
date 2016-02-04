using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RyzePro.Modes
{
    class Combo
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

            if (PassiveStack <= 2)
            {
                if (Checker.ComboUseQ && Spells.Q.IsReady() && Target.IsValidTarget(Spells.Q.Range))
                {
                    Spells.Q.Cast(Target);
                }

                if (Checker.ComboUseE && Spells.E.IsReady() && Target.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(Target);
                }
                if (Checker.ComboUseW && Spells.W.IsReady() && Target.IsValidTarget(Spells.W.Range))
                {
                    Spells.W.Cast(Target);
                }

                if (Checker.ComboUseR && Spells.R.IsReady() && Target.HasBuff("RyzeW") && Target.IsValidTarget(Spells.W.Range))
                {
                    Spells.R.Cast();
                }
            }
            else if (PassiveStack == 3)
            {
                if (Checker.ComboUseW && Spells.W.IsReady() && Target.IsValidTarget(Spells.W.Range))
                {
                    Spells.W.Cast(Target);
                }

                if (Checker.ComboUseQ && Spells.Q.IsReady() && Target.IsValidTarget(Spells.Q.Range))
                {
                        Spells.Q.Cast(Target);
                }

                if (Checker.ComboUseE && Spells.E.IsReady() && Target.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(Target);
                }

                if (Checker.ComboUseQ && Spells.Q.IsReady() && Target.IsValidTarget(Spells.Q.Range))
                {
                        Spells.Q.Cast(Target);
                }

                if (Checker.ComboUseR && Spells.R.IsReady() && Target.HasBuff("RyzeW") && Target.IsValidTarget(Spells.W.Range))
                {
                    Spells.R.Cast();
                }
            }
            else if (PassiveStack == 4)
            {
                if (Checker.ComboUseW && Spells.W.IsReady() && Target.IsValidTarget(Spells.W.Range))
                {
                    Spells.W.Cast(Target);
                }

                if (Checker.ComboUseQ && Spells.Q.IsReady() && Target.IsValidTarget(Spells.Q.Range))
                {
                    Spells.Q.Cast(Target);
                }

                if (Checker.ComboUseE && Spells.E.IsReady() && Target.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(Target);
                }
            }
        }

    }
}

