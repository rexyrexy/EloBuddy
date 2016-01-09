using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.Menu.Values;

namespace First_Class_Rengar
{
    public class MenuInit
    {
        public static Menu ComboMenu, ClearMenu, MainnMenu, JungleClear, Heal, KillSteal, BetaMenu, MiscMenu;
        public static void Initialize()
        {

            MainnMenu = MainMenu.AddMenu("FirstRengar", "rebornrengar");
            MainnMenu.AddGroupLabel("FirstRengar");
            MainnMenu.AddLabel("Coded by Rexy");

            ComboMenu = MainnMenu.AddSubMenu("Combo Menu", "Combo");
            Heal = MainnMenu.AddSubMenu("Heal", "Heal");
            KillSteal = MainnMenu.AddSubMenu("Killsteal", "Killsteal");
            BetaMenu = MainnMenu.AddSubMenu("Beta Options", "Beta");
            MiscMenu = MainnMenu.AddSubMenu("Drawings Menu", "DrawingMenu");
            ComboMenu.Add("Combo.Use.Q",new CheckBox("Use Q"));
            ComboMenu.Add("Combo.Use.W", new CheckBox("Use W"));
            ComboMenu.Add("Combo.Use.E", new CheckBox("Use E"));
            ComboMenu.Add("Combo.Use.E.OutOfRange", new CheckBox("Use E when out of range"));
            var cs = ComboMenu.Add("css", new Slider("Prioritize",2,0,2));
            var co = new[] { "E", "W", "Q" };
            cs.DisplayName = co[cs.CurrentValue];
            cs.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = co[changeArgs.NewValue];
                };
            Heal.Add("Heal.AutoHeal", new CheckBox("Heal Yourself Auto"));
            Heal.Add("Heal.HP", new Slider("Heal Auto at HP %x percent", 30, 1, 100));
            KillSteal.Add("Killsteal.On", new CheckBox("KillSteal On"));
            KillSteal.AddLabel("This will use W for KillSteal");
            BetaMenu.Add("Beta.Cast.Q", new CheckBox("Use Beta Q"));
            BetaMenu.Add("Beta.Cast.Q.Delay", new Slider("Q Cast Delay", 500, 100, 2000));
            BetaMenu.Add("Beta.searchrange.Q", new Slider("Q Cast Range", 1000, 500, 1500));
            BetaMenu.AddLabel("Default Assasin Search Range is 1500");
            MiscMenu.Add("Misc.Drawings.Off", new CheckBox("Turn off drawings", false));
            MiscMenu.Add("Misc.Drawings.Prioritized", new CheckBox("Draw Priority"));
            MiscMenu.Add("Misc.Drawings.W", new CheckBox("Draw W"));
            MiscMenu.Add("Misc.Drawing.E", new CheckBox("Draw E"));
        }
    }
}
