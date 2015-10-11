using System;
using System.Linq;
using BrainDotExe.Util;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using System.Collections.Generic;
using SharpDX;
using Color = System.Drawing.Color;
using Utility = BrainDotExe.Common.Utility;

namespace BrainDotExe.Draw
{
    class TowerUtil
    {
        public static Menu TowerUtilMenu;

        public static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        public static void Init()
        {
            TowerUtilMenu = Program.Menu.AddSubMenu("Tower Util ", "towerUtilDraw");
            TowerUtilMenu.AddGroupLabel("Tower Ranges");
            TowerUtilMenu.Add("drawRanges", new CheckBox("Draw Ranges", false));
            TowerUtilMenu.AddGroupLabel("Tower Life");
            TowerUtilMenu.Add("drawTurretLife", new CheckBox("Draw Turrets Life", false));
            TowerUtilMenu.Add("drawLifeEnemy", new CheckBox("Draw Enemy Life", false));
            TowerUtilMenu.Add("drawLifeAlly", new CheckBox("Draw Ally Life", false));

            Drawing.OnDraw += TowerUtil_OnDraw;
            Drawing.OnEndScene += TowerUtil_OnEndScene;
        }

        public static void TowerUtil_OnDraw(EventArgs args)
        {
            if (Misc.isChecked(Program.DrawMenu, "drawDisable")) return;

            if (Misc.isChecked(TowerUtilMenu, "drawRanges"))
            {
                foreach (var turret in ObjectManager.Get<Obj_Turret>().Where(a => a.IsEnemy).Where(a => !a.IsDead).Where(a => _Player.Distance(a) <= 2000).Where(a => a.Health >= 1))
                {
                    if (_Player.Distance(turret) <= 870)
                        new Circle() { Color = Color.Red, Radius = 870, BorderWidth = 2f }.Draw(turret.Position);
                    if (_Player.Distance(turret) > 870 && _Player.Distance(turret) < 1650)
                        new Circle() { Color = Color.Yellow, Radius = 870, BorderWidth = 2f }.Draw(turret.Position);
                    if (_Player.Distance(turret) >= 1650)
                        new Circle() { Color = Color.White, Radius = 870, BorderWidth = 2f }.Draw(turret.Position);

                }
            }
        }

        public static void TowerUtil_OnEndScene(EventArgs args)
        {
            if (Misc.isChecked(TowerUtilMenu, "drawTurretLife"))
            {
                foreach (var turret in ObjectManager.Get<Obj_Turret>().Where(a => !a.IsDead).Where(a => !Utility.InFountain(a.Position.To2D())))
                {
                    if (Misc.isChecked(TowerUtilMenu, "drawLifeEnemy") && turret.Team != _Player.Team)
                    {
                        Drawing.DrawText(Drawing.WorldToMinimap(turret.Position).X,Drawing.WorldToMinimap(turret.Position).Y, Color.White, Math.Round(turret.HealthPercent) + "%"); // infos do escrito
                    }
                    if (Misc.isChecked(TowerUtilMenu, "drawLifeAlly") && turret.Team == _Player.Team)
                    {
                        Drawing.DrawText(Drawing.WorldToMinimap(turret.Position).X, Drawing.WorldToMinimap(turret.Position).Y, Color.White, Math.Round(turret.HealthPercent) + "%"); // infos do escrito
                    }
                }
            }
        }

    }
}
