using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace RyzePro
{
    class Menu
    {
        public static EloBuddy.SDK.Menu.Menu MaiinMenu, ComboMenu, LaneClearMenu, JungleClearMenu, HarassMenu, LastHitMenu, DrawMenu, HumanizerMenu;
        public static void Init()
        {
            MaiinMenu = MainMenu.AddMenu("RyzePro", "RyzePro");
            MaiinMenu.AddGroupLabel("Ryze Pro Loaded..");
            MaiinMenu.AddLabel("Coded by Rexy");
            MaiinMenu.AddLabel("If you found a bug, Pls feedback on my thread");

            ComboMenu = MaiinMenu.AddSubMenu("Combo");
            ComboMenu.Add("combo.useQ", new CheckBox("Use Q"));
            ComboMenu.Add("combo.useW", new CheckBox("Use W"));
            ComboMenu.Add("combo.useE", new CheckBox("Use E"));
            ComboMenu.Add("combo.useR", new CheckBox("Use R when target rooted"));
            ComboMenu.Add("combo.aa", new CheckBox("Dont AA while combo", false));
            ComboMenu.Add("combo.gapcloser", new CheckBox("Auto GapCloser Enemy"));

            HarassMenu = MaiinMenu.AddSubMenu("Harass");
            HarassMenu.Add("harass.useQ", new CheckBox("Use Q"));
            HarassMenu.Add("harass.useW", new CheckBox("Use W"));
            HarassMenu.Add("harass.useE", new CheckBox("Use E"));
            HarassMenu.Add("harass.mana", new Slider("Min % Mana", 55));

            LaneClearMenu = MaiinMenu.AddSubMenu("LaneClear");
            LaneClearMenu.Add("laneclear.useQ", new CheckBox("Use Q"));
            LaneClearMenu.Add("laneclear.useW", new CheckBox("Use W"));
            LaneClearMenu.Add("laneclear.useE", new CheckBox("Use E"));
            LaneClearMenu.Add("laneclear.useR", new CheckBox("Use R"));
            LaneClearMenu.Add("laneclear.mana", new Slider("Min % Mana", 55));

            JungleClearMenu = MaiinMenu.AddSubMenu("JungleClear");
            JungleClearMenu.Add("jungleclear.useQ", new CheckBox("Use Q"));
            JungleClearMenu.Add("jungleclear.useW", new CheckBox("Use W"));
            JungleClearMenu.Add("jungleclear.useE", new CheckBox("Use E"));
            JungleClearMenu.Add("jungleclear.useR", new CheckBox("Use R"));
            JungleClearMenu.Add("jungleclear.mana", new Slider("Min % Mana", 55));

            LastHitMenu = MaiinMenu.AddSubMenu("LastHit");
            LastHitMenu.Add("lasthit.useQ", new CheckBox("Use Q"));

            DrawMenu = MaiinMenu.AddSubMenu("Draw Settings");
            DrawMenu.Add("draw.no", new CheckBox("No Drawings"));
            DrawMenu.Add("draw.Q", new CheckBox("Draw Q range"));
            DrawMenu.Add("draw.W", new CheckBox("Draw W range"));
            DrawMenu.Add("draw.E", new CheckBox("Draw E range"));

            HumanizerMenu = MaiinMenu.AddSubMenu("Humanizer");
            HumanizerMenu.Add("humanizer.active", new CheckBox("Humanizer Active", false));
            HumanizerMenu.Add("humanizer.mindelay", new Slider("Min Delay", 150, 0, 400));
            HumanizerMenu.Add("humanizer.maxdelay", new Slider("Max Delay", 250, 0, 500));
        }
    }
}
