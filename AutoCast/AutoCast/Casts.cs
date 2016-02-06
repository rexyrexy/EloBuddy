using System;
using EloBuddy;

namespace AutoCast
{
    class Casts
    {
        public static void Cast()
        {
            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Items.GetSmite();
            Items.CastHydra();
            Items.CastTiamat();
            Items.CastSmite();
        }
    }
}
