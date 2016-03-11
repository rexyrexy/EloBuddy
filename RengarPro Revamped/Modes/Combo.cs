using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using RengarPro_Revamped.Helper;

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

            Orbwalker.ForcedTarget = EnemyTarget;
            QPriority(EnemyTarget);

            switch (MenuChecker.ComboModeSelected)
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
                            CastSmite(Misc.Smite, EnemyTarget);
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(EnemyTarget);
                            }
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                
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
                                CastEPrediction(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();

                            }
                        }
                        if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady() && MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            CastEPrediction(EnemyTarget);
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
                                CastEPrediction(EnemyTarget);
                            }
                            if (EnemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (E.IsReady() && EnemyTarget.IsValidTarget(E.Range))
                            {
                                CastEPrediction(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (EnemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                
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
                                CastEPrediction(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(EnemyTarget);
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Ap Rengoo
                        if (!RengarHasPassive && Ferocity == 4)
                        {
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }
                        if (RengarHasPassive && Ferocity == 4)
                        {
                            if (EnemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }
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
                                CastEPrediction(EnemyTarget);
                            }

                            if (Q.IsReady() && EnemyTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                                
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
                                CastEPrediction(EnemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (W.IsReady() && EnemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                        }
                        if (EnemyTarget.IsValidTarget(E.Range) && E.IsReady() && MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            CastEPrediction(EnemyTarget);
                        }
                    }
                    break;
            }
        }

        internal static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var target = TargetSelector.GetTarget(1500, DamageType.Physical);
            if (!target.IsValidTarget())
            {
                return;
            }

            if (Ferocity == 5 && Helper.MenuChecker.ComboModeSelected == 1)
            {
                if (Q.IsReady())
                {
                    Q.Cast();
                }
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Ferocity == 5)
                {
                    switch (MenuChecker.ComboModeSelected)
                    {
                        case 2:
                            if (E.IsReady() && target.IsValidTarget(E.Range))
                            {
                                CastEPrediction(target);
                            }
                            break;
                        case 1:
                            if (Q.IsReady() && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                            }

                            if (target.IsValidTarget(Q.Range))
                            {
                                Core.DelayAction((() =>
                                {
                                    if (target.IsValidTarget(W.Range))
                                    {
                                        W.Cast();
                                    }

                                    CastEPrediction(target);
                                    CastItems();
                                }), 50);
                            }
                            break;
                    }
                }

                switch (MenuChecker.ComboModeSelected)
                {
                    case 2:
                        if (E.IsReady() && target.IsValidTarget(E.Range))
                        {
                            CastEPrediction(target);
                        }
                        break;

                    case 1:
                        var BetaQActive = Menu.ComboM["betaq.active"].Cast<CheckBox>().CurrentValue;
                        var BetaQRange = Menu.ComboM["betaq.range"].Cast<Slider>().CurrentValue;
                        if (BetaQActive && target.IsValidTarget(BetaQRange) && RengarHasUltimate)
                        {
                            Q.Cast();
                        }
                        break;
                }

                if (e.Duration - 100 - Game.Ping/2 > 0)
                {
                    Core.DelayAction((() => CastItems()), e.Duration - 100 - Game.Ping / 2);
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
            var QPrioActive = Menu.ComboM["qprio.active"].Cast<CheckBox>().CurrentValue;
            if (!QPrioActive) { return; }
            if (RengarQ && !RengarHasPassive && !Q.IsOnCooldown)
            {
                if (Target.IsValidTarget(Q.Range)) { 
                Player.IssueOrder(GameObjectOrder.AttackUnit, Target);
            }
            }
        }

        public static void CastEPrediction(AIHeroClient eCastTarget)
        {
            var predictione = E.GetPrediction(eCastTarget);
            if (predictione.HitChance >= HitChance.High && predictione.CollisionObjects.Count() == 0)
            {
                E.Cast(eCastTarget);
            }
            else if (predictione.HitChance < HitChance.High && predictione.CollisionObjects.Count() == 0)
            {
                E.Cast(eCastTarget.ServerPosition);
            }
        }
    }
}
