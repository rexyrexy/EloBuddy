using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped.Modes
{
    class Combo : Standarts
    {
        private static AIHeroClient EnemyTarget;
        public static void Do()
        {
            if (!TargetSelector.SelectedTarget.IsValidTarget(1750))
            {
                EnemyTarget = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            }
            else if (TargetSelector.SelectedTarget.IsValidTarget(1750))
            {
                EnemyTarget = TargetSelector.SelectedTarget;
            }

            if (RengarHasUltimate)
            {
                return;
            }

            QPriority(EnemyTarget);

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
                                Orbwalking.ResetAutoAttackTimer();
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalking.ResetAutoAttackTimer();
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalking.ResetAutoAttackTimer();
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
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();

                            }
                        }
                        if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady() && Helper.MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
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
                                Orbwalking.ResetAutoAttackTimer();
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
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                Orbwalking.ResetAutoAttackTimer();
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
                case 3:
                    {
                        //Ap Rengoo
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (W.IsReady() && EnemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (E.IsReady() && EnemyTarget.IsValidTarget(E.Range))
                            {
                                E.Cast(EnemyTarget);
                            }

                            if (Q.IsReady() && EnemyTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                                Orbwalking.ResetAutoAttackTimer();
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }   
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (Q.IsReady() && EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()))
                            {
                                Q.Cast();
                                Orbwalking.ResetAutoAttackTimer();
                            }
                            if (W.IsReady() && EnemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                            if (EnemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Helper.Misc.Smite, EnemyTarget);
                            if (E.IsReady() && EnemyTarget.IsValidTarget(E.Range))
                            {
                                E.Cast(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (W.IsReady() && EnemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                        }
                        if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady() && Helper.MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            E.Cast(EnemyTarget);
                        }
                    }
                    break;
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
        public static void CastSmite(SpellSlot smiteSlotx, AIHeroClient target)
        {
            if (Helper.MenuChecker.ComboSmiteActive && !RengarHasUltimate && Helper.Misc.Smite != SpellSlot.Unknown
                    && Rengar.Spellbook.CanUseSpell(Helper.Misc.Smite) == SpellState.Ready && target.IsValidTarget(500))
            {
                Rengar.Spellbook.CastSpell(smiteSlotx, target);
            }
        }
        public static void QPriority(AIHeroClient Target)
        {
            if (RengarQ)
            {
                Player.IssueOrder(GameObjectOrder.AttackTo, Target);
            }
        }
    }
}
