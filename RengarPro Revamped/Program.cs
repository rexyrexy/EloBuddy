using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK;
using Color = System.Drawing.Color;

namespace RengarPro_Revamped
{
    class Program : Standarts
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Rengar.Hero != Champion.Rengar)
            {
                return;
            }
            ModeChecker.Do();
            Menu.Init();
            Helper.Misc.Init();
            Helper.Magnet.Initialize();
            Helper.Targetting.Initialize();
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && !RengarHasPassive && Q.IsReady()
                    && !(Helper.MenuChecker.ComboModeSelected == 2 || Helper.MenuChecker.ComboModeSelected == 3 && Ferocity == 5))
            {
                 if (Player.Instance.Position.To2D().Distance(target.Position.To2D())
                    >= Player.Instance.BoundingRadius + Player.Instance.AttackRange + args.Target.BoundingRadius)
                {
                    args.Process = false;
                    Q.Cast();
                }
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!target.IsMe || target == null || !(target is AIHeroClient))
            {
                return;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast();
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Helper.MenuChecker.DrawComboModeActive)
            {
                switch (Helper.MenuChecker.ComboModeSelected)
                {
                    case 1:
                        {
                            Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : One Shot");
                            break;
                        }
                    case 2:
                        {
                            Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : Snare");
                            break;
                        }
                    case 3:
                        {
                            Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : AP Rengo");
                            break;
                        }
                }
            }

            if (Helper.MenuChecker.DrawSelectedEnemyActive && TargetSelector.SelectedTarget.IsValidTarget() && TargetSelector.SelectedTarget.IsVisible && !TargetSelector.SelectedTarget.IsDead && !(TargetSelector.SelectedTarget.IsMinion || TargetSelector.SelectedTarget.IsMonster || TargetSelector.SelectedTarget is Obj_AI_Turret))
            {
                Drawing.DrawText(
                Drawing.WorldToScreen(TargetSelector.SelectedTarget.Position).X - 40,
                Drawing.WorldToScreen(TargetSelector.SelectedTarget.Position).Y + 10,
                Color.White,
                "Selected Target");
            }
        }
    }
}
