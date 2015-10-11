using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using BrainDotExe.Common;
using Color = System.Drawing.Color;

namespace BrainDotExe.Draw
{
    class BlinkDetector
    {
        public static Menu BlinkDetectorMenu;

        public static List<Tuple<float, Vector3>> times = new List<Tuple<float, Vector3>>();

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            BlinkDetectorMenu = Program.Menu.AddSubMenu("Blink Detector ", "blinkDetectorDraw");
            BlinkDetectorMenu.AddGroupLabel("Blink Detector");
            BlinkDetectorMenu.Add("drawEnd", new CheckBox("Show where he was", false));
            BlinkDetectorMenu.Add("drawSeconds", new Slider("Show Position for seconds ", 5, 1, 20));

            Drawing.OnDraw += BlinkDetector_OnDraw;
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
        }

        public static void BlinkDetector_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(BlinkDetectorMenu, "drawEnd"))
            {
                var auxtimes = new List<Tuple<float, Vector3>>();
                foreach (var time in times)
                {
                    var diffTime = time.Item1 - Game.Time;
                    if (diffTime > 0)
                    {
                        new Circle() { Color = Color.Yellow, Radius = 100f, BorderWidth = 2f }.Draw(time.Item2);
                    }
                    else
                    {
                        auxtimes.Add(time);
                    }
                }

                if (auxtimes.Count > 0)
                {
                    times = times.Except(auxtimes).ToList();
                }
            }
        }

        private static void OnProcessSpellCast(Obj_AI_Base caster, GameObjectProcessSpellCastEventArgs args)
        {
            if (!caster.IsChampion()) return;

            if (args.SData.Name == "EzrealArcaneShift" || args.SData.Name == "summonerflash")
            {
                var timer = new Tuple<float, Vector3>(Game.Time + Misc.getSliderValue(BlinkDetectorMenu, "drawSeconds"), args.End);
                times.Add(timer);
            }

        }

    }
}
