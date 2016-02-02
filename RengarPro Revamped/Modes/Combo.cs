using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace RengarPro_Revamped.Modes
{
    class Combo : Standarts
    {
        private static AIHeroClient EnemyTarget;
        public static void Do()
        {
            if (!TargetSelector.SelectedTarget.IsValidTarget(R.Range))
            {
                EnemyTarget = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            }
            else if (TargetSelector.SelectedTarget.IsValidTarget(R.Range))
            {
                EnemyTarget = TargetSelector.SelectedTarget;
            }

            if (RengarHasUltimate)
            {
                return;
            }

            switch (Helper.MenuChecker.ComboModeSelected)
            { 
                case 1:
                    {
                        //One Shot Mode Logic
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                E.Cast(EnemyTarget);
                            }
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(600) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                E.Cast(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(600) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                        if (EnemyTarget.IsValidTarget(1000) && E.IsReady() && Helper.MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            E.Cast(EnemyTarget);
                        }
                        break;
                    }
                        case 2:
                    {
                        //Snare Logic
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                E.Cast(EnemyTarget);
                            }
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (E.IsReady() && EnemyTarget.IsValidTarget(E.Range))
                            {
                                E.Cast(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(600) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalker.ResetAutoAttack();
                            }
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                E.Cast(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                E.Cast(EnemyTarget);
                            }
                        }
                        break;
                    }
                }
            }
        public static void CastItems()
        {
            if ( !(Item.CanUseItem(ItemId.Ravenous_Hydra_Melee_Only) || Item.CanUseItem(ItemId.Tiamat_Melee_Only)))
            {
                return;
            }

            Item.UseItem(ItemId.Ravenous_Hydra_Melee_Only);
            Item.UseItem(ItemId.Tiamat_Melee_Only);
        }
        public static void CastSmite(SpellSlot smiteSlotx, AIHeroClient target)
        {
                if (Helper.MenuChecker.ComboSmiteActive && !RengarHasUltimate && Helper.Misc.Smite != SpellSlot.Unknown
                        && Rengar.Spellbook.CanUseSpell(Helper.Misc.Smite) == SpellState.Ready && target.IsValidTarget(500))
                {
                    Rengar.Spellbook.CastSpell(smiteSlotx, target);
                }
            }
        }
}
