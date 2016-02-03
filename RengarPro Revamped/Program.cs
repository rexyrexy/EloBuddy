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
            if (Standarts.Rengar.Hero != Champion.Rengar)
            {
                return;
            }
            
            Menu.Init();
            Drawing.OnDraw += Drawing_OnDraw;
            Helper.Misc.Init();
            ModeChecker.Do();
            Dash.OnDash += Dash_OnDash;
            Helper.Magnet.Initialize();
            Helper.Targetting.Initialize();
        }
        

        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var target = TargetSelector.GetTarget(1500, DamageType.Physical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Standarts.Ferocity == 5)
                {
                    switch (Helper.MenuChecker.ComboModeSelected)
                    {
                        case 2:
                            if (E.IsReady() && target.IsValidTarget(E.Range))
                            {
                                E.Cast(target);
                            }
                            break;
                        case 1:
                            if (Q.IsReady() && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }

                            if (target.IsValidTarget(Q.Range))
                            {

                                Core.DelayAction(() =>
                                {
                                    if (target.IsValidTarget(W.Range))
                                    {
                                        W.Cast();
                                    }
                                    E.Cast(target);
                                    Modes.Combo.CastItems();
                                },
                               50);
 
                            }

                            break;
                    }
                }

                switch (Helper.MenuChecker.ComboModeSelected)
                {
                    case 2:
                        if (E.IsReady() && target.IsValidTarget(E.Range))
                        {
                            E.Cast(target);
                        }
                        break;

                    case 1:
                        if (RengarHasUltimate && Q.IsReady())
                        {
                            Q.Cast();
                            Orbwalker.ResetAutoAttack();
                        }
                        break;
                }

                if (e.Duration - 100 - Game.Ping / 2 > 0)
                {
                    Core.DelayAction(() => Modes.Combo.CastItems(), e.Duration - 100 - Game.Ping / 2);
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
