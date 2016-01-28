using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped.Modes
{
    class LaneClear : Standarts
    {
        public static void Do()
        {
            var laneTarget = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => !x.IsDead && W.IsInRange(x));

            if (Ferocity < 5 || (Ferocity == 5 && !Helper.MenuChecker.LaneClearSaveStacks))
            {
                if (Helper.MenuChecker.LaneClearUseW && W.IsReady() && laneTarget.IsValidTarget())
                {
                    W.Cast();
                }
                if (Helper.MenuChecker.LaneClearUseQ && Q.IsReady() && laneTarget.IsValidTarget())
                {
                    Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
                if (laneTarget.IsValidTarget(Rengar.GetAutoAttackRange()))
                {
                    CastItems();
                }
                if (Helper.MenuChecker.LaneClearUseE && E.IsReady() && laneTarget.IsValidTarget())
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
