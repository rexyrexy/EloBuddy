using EloBuddy;
using EloBuddy.SDK;

namespace RyzePro.Modes
{
    class Combo
    {
        private static Spell.SpellBase Q = Spells.Q;
        private static Spell.SpellBase W = Spells.W;
        private static Spell.SpellBase E = Spells.E;
        private static Spell.SpellBase R = Spells.R;

        private static bool QReady
        {
            get { return Q.IsReady(); }
        }
        private static bool WReady
        {
            get { return W.IsReady(); }
        }
        private static bool EReady
        {
            get { return E.IsReady(); }
        }
        private static bool RReady
        {
            get { return R.IsReady(); }
        }

        private static bool IsValid(Spell.SpellBase slot)
        {
            return Target.IsValidTarget(slot.Range);
        }
        private static AIHeroClient Target;

        private static bool IsRooted(AIHeroClient target)
        {
             return target.HasBuff("RyzeW") || target.IsRooted;
        }
        public static void Do()
        {
            if (TargetSelector.SelectedTarget == null)
            {
                Target = TargetSelector.GetTarget(Spells.Q.Range + 250, DamageType.Magical);
            }
            else if (TargetSelector.SelectedTarget != null)
            {
                Target = TargetSelector.SelectedTarget;
            }

            if (Target == null)
            {
                return;
            }
            
                    if (QReady && IsValid(Q) && Checker.ComboUseQ)
                    {
                        Q.Cast(Target);
                    }
                    if (RReady && IsRooted(Target) && Checker.ComboUseR)
                    {
                        R.Cast();
                    }
                    if (EReady && IsValid(E) && Checker.ComboUseE)
                    {
                        E.Cast(Target);
                    }
                    if (WReady && IsValid(W) && Checker.ComboUseW)
                    {
                        W.Cast(Target);
                    }
        }

    }
}

