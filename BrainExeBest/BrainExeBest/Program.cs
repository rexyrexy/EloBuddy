using System;
using BrainDotExe.Draw;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using BrainDotExe.Util;

namespace BrainDotExe
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Game_OnStart;
        }

        private static void Game_OnStart(EventArgs args)
        {
            Menu = MainMenu.AddMenu("Brain.exe", "braindotexe");
            Menu.AddSeparator();
            Menu.AddLabel("By KK2 & MrArticuno");

            DrawMenu = Menu.AddSubMenu("Draw", "brainDraw");
            DrawMenu.Add("drawDisable", new CheckBox("Turn off all drawings", false));
            DrawMenu.Add("streamMode", new CheckBox("Stream Mode", false));

            JungleTimers.Init();
            Cooldown.Init();
            CloneRevelaer.Init();
            Pink.Init();
            TowerUtil.Init();
            SmiteME.Init();
            BlinkDetector.Init();
            WardTracker.Init();

        }

        #region Variaveis

        /*
         Config
         */

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static String G_version = "1.4.0";
        public static String G_charname = _Player.ChampionName;

        /*
         Menus
         */

        public static Menu Menu,
            DrawMenu;

        #endregion
    }
}
