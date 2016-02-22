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
            
            Drawing.OnDraw += Drawing_OnDraw;
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
