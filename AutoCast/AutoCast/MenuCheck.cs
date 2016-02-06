using EloBuddy.SDK.Menu.Values;

namespace AutoCast
{
    public static class MenuCheck
    {
        public static bool CastHydra
        {
            get
            {
                return MenuInit.CastMenu["cast.hydra"].Cast<CheckBox>().CurrentValue;
            }
        }
        public static bool CastTiamat
        {
            get
            {
                return MenuInit.CastMenu["cast.tiamat"].Cast<CheckBox>().CurrentValue;
            }
        }
        public static bool CastSmite
        {
            get
            {
                return MenuInit.CastMenu["cast.smite"].Cast<CheckBox>().CurrentValue;
            }
        }
    }
}
