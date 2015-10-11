using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace BrainDotExe.Draw
{
    class CloneRevelaer
    {
        public static Menu CloneRevealerMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            CloneRevealerMenu = Program.Menu.AddSubMenu("Clone Revealer", "cloneRevealerDraw");
            CloneRevealerMenu.AddGroupLabel("Clone Revealer");
            CloneRevealerMenu.Add("drawClones", new CheckBox("Show Clones", true));

            Drawing.OnDraw += AttackRange_OnDraw;
        }

        public static void AttackRange_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(CloneRevealerMenu, "drawClones"))
            {
                foreach (var enemy in ObjectManager.Get<Obj_AI_Minion>().Where(a => a.IsEnemy).Where(a => !a.IsDead).Where(a => a.BaseSkinName.ToLower().Contains("shaco") || a.BaseSkinName.ToLower().Contains("leblanc") || a.BaseSkinName.ToLower().Contains("monkeyking")))
                {
                    Drawing.DrawText(Drawing.WorldToScreen(enemy.Position) - new Vector2(30, 0), Color.Red, "This is a Clone", 15);
                    new Circle() { Color = Color.Red, Radius = 30, BorderWidth = 2f }.Draw(enemy.Position);
                }
            }
        }
    }
}
