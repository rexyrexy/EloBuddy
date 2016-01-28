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
                    if (Helper.MenuChecker.JungleClearUseQ && Q.IsReady() && Rengar.Distance(jungleMinion) < Rengar.AttackRange)
                    {
                        Q.Cast();
                        Orbwalker.ResetAutoAttack();
                    }
                    if (jungleMinion.IsValidTarget(Rengar.GetAutoAttackRange()))
                    {
                       CastItems();
                    }
                    if (Helper.MenuChecker.JungleClearUseW && W.IsReady() && Rengar.Distance(jungleMinion) <= W.Range)
                    {
                        W.Cast();
                    }
                    if (Helper.MenuChecker.JungleClearUseE && E.IsReady() && Rengar.Distance(jungleMinion) <= E.Range)
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
