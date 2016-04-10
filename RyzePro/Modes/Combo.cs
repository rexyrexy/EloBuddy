using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace RyzePro.Modes
{
    class Combo
    {
        private static Spell.SpellBase Q = Spells.Q;
        private static Spell.SpellBase W = Spells.W;
        private static Spell.SpellBase E = Spells.E;
        private static Spell.SpellBase R = Spells.R;



        public static bool QReady
        {
            get { return Q.IsReady(); }
        }
        public static bool WReady
        {
            get { return W.IsReady(); }
        }
        public static bool EReady
        {
            get { return E.IsReady(); }
        }
        public static bool RReady
        {
            get { return R.IsReady(); }
        }

        private static void QCast(Obj_AI_Base target)
        {
            if (QReady && IsValid(Q, target) && Checker.ComboUseQ)
            {
                Q.Cast(target);
            }
        }

        private static void WCast(Obj_AI_Base target)
        {
            if (WReady && IsValid(W, target) && Checker.ComboUseW)
            {
                W.Cast(target);
            }
        }

        private static void ECast(Obj_AI_Base target)
        {
            if (EReady && IsValid(E, target) && Checker.ComboUseE)
            {
                E.Cast(target);
            }
        }

        private static void RCast(Obj_AI_Base target)
        {
            if (RReady && IsRooted(target) && IsValid(W, target) && Checker.ComboUseR)
            {
                R.Cast();
            }
        }

        private static bool IsValid(Spell.SpellBase slot, Obj_AI_Base target)
        {
            return target.IsValidTarget(slot.Range);
        }
        private static Obj_AI_Base Target;

        private static bool IsRooted(Obj_AI_Base target)
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
                    QCast(Target);
                    RCast(Target);
                    ECast(Target);
                    WCast(Target);
            }        
        }
    }

