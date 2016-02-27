using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped.Modes
{
    class JungleClear : Standarts
    {
        public static void Do()
        {
            foreach (var jungleMinion in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (Ferocity < 5 || (Ferocity == 5 && !Helper.MenuChecker.JungleClearSaveStacks))
                {
                    if (Helper.MenuChecker.JungleClearUseQ && Ferocity == 5)
                    {
                        if (Q.IsReady())
                        {
                            Q.Cast();
                        }
                    }
                    if (Helper.MenuChecker.JungleClearUseQ && Q.IsReady() && Q.IsInRange(jungleMinion))
                    {
                        Q.Cast();
                    }
                    if (jungleMinion.IsInAutoAttackRange(Rengar))
                    {
                       CastItems();
                    }
                    if (Helper.MenuChecker.JungleClearUseW && W.IsReady() && W.IsInRange(jungleMinion))
                    {
                        W.Cast();
                    }
                    if (Helper.MenuChecker.JungleClearUseE && E.IsReady() && E.IsInRange(jungleMinion))
                    {
                        E.Cast(jungleMinion);
                    }
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
