using System;
using EloBuddy;

namespace AutoCast
{
    class Casts
    {
        public static void Cast()
        {
            if (!Player.Instance.IsVisible)
            {
                return;
            }
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (!Player.Instance.IsVisible)
            {
                return;
            }
            Items.GetSmite();
            Items.CastHydra();
            Items.CastTiamat();
            Items.CastSmite();
        }
    }
}
