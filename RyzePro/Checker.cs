using EloBuddy.SDK.Menu.Values;

namespace RyzePro
{
    class Checker
    {
        public static bool ComboUseQ
        {
            get { return Menu.ComboMenu["combo.useQ"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool ComboUseW
        {
            get { return Menu.ComboMenu["combo.useW"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool ComboUseE
        {
            get { return Menu.ComboMenu["combo.useE"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool ComboUseR
        {
            get { return Menu.ComboMenu["combo.useR"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool WhileComboAa
        {
            get { return Menu.ComboMenu["combo.aa"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool HarassUseQ
        {
            get { return Menu.HarassMenu["harass.useQ"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool HarassUseW
        {
            get { return Menu.HarassMenu["harass.useW"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool HarassUseE
        {
            get { return Menu.HarassMenu["harass.useE"].Cast<CheckBox>().CurrentValue; }
        }

        public static int HarassMana
        {
            get { return Menu.HarassMenu["harass.mana"].Cast<Slider>().CurrentValue; }
        }

        public static bool LaneClearUseQ
        {
            get { return Menu.LaneClearMenu["laneclear.useQ"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool LaneClearUseW
        {
            get { return Menu.LaneClearMenu["laneclear.useW"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool LaneClearUseE
        {
            get { return Menu.LaneClearMenu["laneclear.useE"].Cast<CheckBox>().CurrentValue; }
        }

        public static int LaneClearMana
        {
            get { return Menu.LaneClearMenu["laneclear.mana"].Cast<Slider>().CurrentValue; }
        }
        public static bool JungleClearUseQ
        {
            get { return Menu.JungleClearMenu["jungleclear.useQ"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool JungleClearUseW
        {
            get { return Menu.JungleClearMenu["jungleclear.useW"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool JungleClearUseE
        {
            get { return Menu.JungleClearMenu["jungleclear.useE"].Cast<CheckBox>().CurrentValue; }
        }

        public static int JungleClearMana
        {
            get { return Menu.JungleClearMenu["jungleclear.mana"].Cast<Slider>().CurrentValue; }
        }

        public static bool LastHitUseQ
        {
            get { return Menu.LastHitMenu["lasthit.useQ"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool DrawNo
        {
            get { return Menu.DrawMenu["draw.no"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool DrawQ
        {
            get { return Menu.DrawMenu["draw.Q"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool DrawW
        {
            get { return Menu.DrawMenu["draw.W"].Cast<CheckBox>().CurrentValue; }
        }
        public static bool DrawE
        {
            get { return Menu.DrawMenu["draw.E"].Cast<CheckBox>().CurrentValue; }
        }
        
        public static bool AntiGapCloser
        {
            get { return Menu.ComboMenu["combo.gapcloser"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool LaneClearUseR
        {
            get { return Menu.LaneClearMenu["laneclear.useR"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool JungleClearUseR
        {
            get { return Menu.JungleClearMenu["jungleclear.useR"].Cast<CheckBox>().CurrentValue; }
        }

        public static bool HumanizerActive
        {
            get { return Menu.HumanizerMenu["humanizer.active"].Cast<CheckBox>().CurrentValue; }
        }

        public static int HumanizerMinValue
        {
            get { return Menu.HumanizerMenu["humanizer.mindelay"].Cast<Slider>().CurrentValue; }
        }

        public static int HumanizerMaxValue
        {
            get { return Menu.HumanizerMenu["humanizer.maxdelay"].Cast<Slider>().CurrentValue; }
        }
    }
}
