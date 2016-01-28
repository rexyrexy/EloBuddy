using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace RengarPro_Revamped
{
    class Menu
    {
        public static EloBuddy.SDK.Menu.Menu RengarMenu, ComboM, LaneM, JungleM, DrawM, MiscM;       
            public static void Init()
        {
            //Main Menu
            RengarMenu = MainMenu.AddMenu("RengarPro Revamped", "RengarProMenu");
            RengarMenu.AddGroupLabel("RengarPro Revamped");
            RengarMenu.AddLabel("Its loaded. Have Fun ! :)");
            //Combo Menu
            ComboM = RengarMenu.AddSubMenu("Combo");
            ComboM.AddGroupLabel("Combo Menu");
            ComboM.AddLabel("1- OneShot | 2- Snare");
            ComboM.Add("combo.mode", new Slider("Combo Mode", 1, 1, 2));
            var switcher = ComboM.Add("switch", new KeyBind("Combo Mode Switcher", false, KeyBind.BindTypes.HoldActive, (uint)'T'));
            switcher.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args1)
            {
                if (args1.NewValue == true)
                {
                    var cast = ComboM["combo.mode"].Cast<Slider>();
                    if (cast.CurrentValue == cast.MaxValue)
                    {
                        cast.CurrentValue = 0;
                    }
                    else
                    {
                        cast.CurrentValue++;
                    }
                }
            };

            ComboM.Add("combo.useEoutofQ", new CheckBox("Use E out Q Range"));

            //Lane Clear Menu
            LaneM = RengarMenu.AddSubMenu("Lane Clear");
            LaneM.AddGroupLabel("Lane Clear Settings");
            LaneM.Add("lane.useQ", new CheckBox("Use Q"));
            LaneM.Add("lane.useW", new CheckBox("Use W"));
            LaneM.Add("lane.useE", new CheckBox("Use E"));
            LaneM.Add("lane.save", new CheckBox("Save Stacks", false));

            //Jungle Clear Menu
            JungleM = RengarMenu.AddSubMenu("Jungle Clear");
            JungleM.AddGroupLabel("Jungle Clear Settings");
            JungleM.Add("jungle.useQ", new CheckBox("Use Q"));
            JungleM.Add("jungle.useW", new CheckBox("Use W"));
            JungleM.Add("jungle.useE", new CheckBox("Use E"));
            JungleM.Add("jungle.save", new CheckBox("Save Stacks", false));

            //Draw  Menu
            DrawM = RengarMenu.AddSubMenu("Draw");
            DrawM.AddGroupLabel("Draw Settings");
            DrawM.Add("draw.mode", new CheckBox("Draw Combo Mode"));
            DrawM.Add("draw.selectedenemy", new CheckBox("Draw Selected Enemy"));

            //Misc Menu
            MiscM = RengarMenu.AddSubMenu("Misc");
            MiscM.AddGroupLabel("Misc Menu");
            MiscM.Add("misc.autoyoumuu", new CheckBox("Auto Youmuu when Ulti"));
            MiscM.Add("misc.magnet", new CheckBox("Enable Magnet"));
            MiscM.Add("misc.smite", new CheckBox("Use Smite On Combo"));
            MiscM.Add("misc.autohp", new CheckBox("Auto HP Active"));
            MiscM.Add("misc.hp.value", new Slider("Auto HP %",30,1,100));
            MiscM.AddLabel("1- HeadHunter 2- NightHunter 3- SSW");
            MiscM.Add("skin.active", new CheckBox("Enable Skin Hack"));
            MiscM.Add("skin.value", new Slider("Skin Hack Value", 1, 1, 3));
        }
    }
}
