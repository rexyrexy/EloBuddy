﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped.Modes
{
    class LaneClear : Standarts
    {
        public static void Do()
        {
            var laneTarget = EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.ServerPosition, 400).OrderByDescending(a => a.MaxHealth).FirstOrDefault();

            if (laneTarget == null)
            {
                return;
            }

            if (Ferocity < 5 || (Ferocity == 5 && !Helper.MenuChecker.LaneClearSaveStacks))
            {
                if (Helper.MenuChecker.LaneClearUseQ && Ferocity == 5)
                {
                    if (Q.IsReady())
                    { 
                    Q.Cast();
                    }
                }
                if (Helper.MenuChecker.LaneClearUseW && W.IsReady())
                {
                    W.Cast();
                }
                if (Helper.MenuChecker.LaneClearUseQ && Q.IsReady())
                {
                    Q.Cast();
                }
                if (laneTarget.IsInAutoAttackRange(Player.Instance))
                {
                    CastItems();
                }
                if (Helper.MenuChecker.LaneClearUseE && E.IsReady())
                {
                    E.Cast(laneTarget);
                }
            }
        }
        public static void CastItems()
        {
            if (!(Item.CanUseItem(ItemId.Ravenous_Hydra_Melee_Only) || Item.CanUseItem(ItemId.Tiamat_Melee_Only)))
            {
                return;
            }

            Item.UseItem(ItemId.Ravenous_Hydra_Melee_Only);
            Item.UseItem(ItemId.Tiamat_Melee_Only);
        }
    }
}
