using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace AutoCast
{
    public static class MenuInit
    {
        public static Menu CastMenu;
        public static void Initialize()
        {
            CastMenu = MainMenu.AddMenu("AutoCast", "AutoCast");
            CastMenu.AddGroupLabel("AutoCast coded by Rexy");
            CastMenu.AddLabel("Normally this addon make for lazy Rengar players like me Kappa");
            CastMenu.AddSeparator();
            CastMenu.AddGroupLabel("Cast Settings");
            CastMenu.Add("cast.hydra", new CheckBox("Cast Hydra"));
            CastMenu.Add("cast.tiamat", new CheckBox("Cast Tiamat"));
            CastMenu.Add("cast.smite", new CheckBox("Cast Smite"));
        }
    }
}
