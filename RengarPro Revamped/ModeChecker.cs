using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped
{
    class ModeChecker
    {
       public static void Do()
        {
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(System.EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Modes.Combo.Do();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Modes.LaneClear.Do();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                Modes.JungleClear.Do();
            }
        }
    }
}
