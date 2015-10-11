using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using Color = System.Drawing.Color;

namespace BrainDotExe.Draw
{
    class WardTracker
    {
        public static Menu WardTrackerMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static List<WardInfo> wards = new List<WardInfo>();

        public static void Init()
        {
            WardTrackerMenu = Program.Menu.AddSubMenu("Ward Tracker", "wardTrackerDraw");
            WardTrackerMenu.AddGroupLabel("Ward Tracker");
            WardTrackerMenu.Add("drawWardsPos", new CheckBox("Show Wards Pos", true));
            WardTrackerMenu.Add("drawWardsTimer", new CheckBox("Show Remaining Time", true));

            Drawing.OnDraw += WardTracker_OnDraw;
            GameObject.OnCreate += GameObjectOnCreate;
            GameObject.OnDelete += GameObjectOnDelete;
        }

        private static void GameObjectOnDelete(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || !sender.Name.Contains("Ward")) return;

            var ward = (Obj_AI_Minion)sender;

            if (ward.IsAlly) return;

            var wardInfo = wards.Where(w => w.Available).FirstOrDefault(w => w.Position == ward.Position);
            if (wardInfo != null)
                wardInfo.Available = false;
        }

        private static void GameObjectOnCreate(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || !sender.Name.Contains("Ward")) return;

            var ward = (Obj_AI_Minion)sender;
            if (ward.IsAlly) return;

            switch (ward.BaseSkinName)
            {
                case "YellowTrinket":
                    wards.Add(new WardInfo(ward.Name, false, true, Game.Time + 60, ward.Position, Color.Green));
                    break;
                case "YellowTrinketUpgrade":
                    wards.Add(new WardInfo(ward.Name, false, true, Game.Time + 120, ward.Position, Color.Green));
                    break;
                case "VisionWard":
                    wards.Add(new WardInfo(ward.Name, true, true, 0, ward.Position, Color.DeepPink));
                    break;
                case "SightWard":
                    wards.Add(new WardInfo(ward.Name, false, true, Game.Time + 180, ward.Position, Color.Green));
                    break;

            }


        }

        public static void WardTracker_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(WardTrackerMenu, "drawWardsPos"))
            {
                var auxList = new List<WardInfo>();
                foreach (var wardInfo in wards)
                {
                    if (!wardInfo.Available)
                    {
                        auxList.Add(wardInfo);
                        continue;
                    }

                    var diffTime = wardInfo.DeleteTimer - Game.Time;

                    if (diffTime > 0 || wardInfo.IsPink)
                    {
                        var fancytimer = string.Format("{0:0}", diffTime);
                        new Circle() { Color = wardInfo.Color, Radius = 20, BorderWidth = 1f }.Draw(wardInfo.Position);

                        if (Misc.isChecked(WardTrackerMenu, "drawWardsTimer") || !wardInfo.IsPink)
                        {

                            Drawing.DrawText(Drawing.WorldToScreen(wardInfo.Position), Color.White, fancytimer,
                                50);
                        }
                    }
                    else
                    {
                        auxList.Add(wardInfo);
                    }
                }

                if (auxList.Count > 0)
                {
                    wards = wards.Except(auxList).ToList();
                }
            }
        }
    }

    public class WardInfo
    {
        public WardInfo(string name, bool isPink, bool available, float deleteTimer, Vector3 position, Color color)
        {
            Name = name;
            IsPink = isPink;
            DeleteTimer = deleteTimer;
            Position = position;
            Color = color;
            Available = available;
        }

        public string Name { get; set; }
        public bool IsPink { get; set; }
        public Color Color { get; set; }
        public float DeleteTimer { get; set; }
        public Vector3 Position { get; set; }
        public bool Available { get; set; }
    }
}
