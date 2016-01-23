using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace RengarPro
{
    public static class Magnet
    {
        public static int LastMove;
        public static bool IsDoingMagnet = false;
        public static void Initialize()
        {
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Program.AllMenu["magnet.enable"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                MagnetSelected();
            }
            else
            {
                IsDoingMagnet = false;
                if (Orbwalker.DisableMovement)
                    Orbwalker.DisableMovement = false;
            }
        }

        private static void MagnetSelected()
        {
            var target = TargetSelector.SelectedTarget;
            if (target.IsValidTarget(300))
            {
                IsDoingMagnet = true;
                if (!Orbwalker.DisableMovement)
                    Orbwalker.DisableMovement = true;
                if (Orbwalker.CanMove && Environment.TickCount - LastMove >= 50)
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, target.Position);
                    LastMove = Environment.TickCount;
                }
            }
            else
            {
                IsDoingMagnet = false;
                if (Orbwalker.DisableMovement)
                    Orbwalker.DisableMovement = false;
            }
        }
    }
}