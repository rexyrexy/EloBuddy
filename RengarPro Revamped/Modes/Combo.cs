using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using RengarPro_Revamped.Helper;

namespace RengarPro_Revamped.Modes
{
    class Combo : Standarts
    {
        private static AIHeroClient _enemyTarget;
        public static void Do()
        {
            try
            {
            if (TargetSelector.SelectedTarget == null)
            {
                _enemyTarget = TargetSelector.GetTarget(R.Range, DamageType.Physical);
            }
            else if (TargetSelector.SelectedTarget != null)
            {
                _enemyTarget = TargetSelector.SelectedTarget;
            }

            if (RengarHasUltimate)
            {
                return;
            }

            Orbwalker.ForcedTarget = _enemyTarget;
            QPriority(_enemyTarget);

            switch (MenuChecker.ComboModeSelected)
            {
                case 1:
                    {
                        //One Shot Mode Logic
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(_enemyTarget);
                            }
                            if (_enemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (_enemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (_enemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(_enemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (_enemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();

                            }
                        }
                        if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady() && MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            CastEPrediction(_enemyTarget);
                        }
                        break;
                    }
                case 2:
                    {
                        //Snare Logic
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(_enemyTarget);
                            }
                            if (_enemyTarget.IsValidTarget(Q.Range) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (E.IsReady() && _enemyTarget.IsValidTarget(E.Range))
                            {
                                CastEPrediction(_enemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (_enemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()) && Q.IsReady())
                            {
                                Q.Cast();
                                
                            }
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(_enemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady())
                            {
                                CastEPrediction(_enemyTarget);
                            }
                        }
                        break;
                    }
                case 3:
                    {
                        //Ap Rengoo
                        if (!RengarHasPassive && Ferocity == 4)
                        {
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }
                        if (RengarHasPassive && Ferocity == 4)
                        {
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }
                        if (!RengarHasPassive && Ferocity <= 4)
                        {
                            if (W.IsReady() && _enemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (E.IsReady() && _enemyTarget.IsValidTarget(E.Range))
                            {
                                CastEPrediction(_enemyTarget);
                            }

                            if (Q.IsReady() && _enemyTarget.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                                
                            }
                        }
                        if (!RengarHasPassive && Ferocity == 5)
                        {
                            if (_enemyTarget.IsValidTarget(W.Range) && W.IsReady())
                            {
                                W.Cast();
                            }
                        }   
                        if (RengarHasPassive && Ferocity <= 4)
                        {
                            if (Q.IsReady() && _enemyTarget.IsValidTarget(Rengar.GetAutoAttackRange()))
                            {
                                Q.Cast();
                                
                            }
                            if (W.IsReady() && _enemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                            if (_enemyTarget.IsValidTarget(400))
                            {
                                CastItems();
                            }
                            CastSmite(Misc.Smite, _enemyTarget);
                            if (E.IsReady() && _enemyTarget.IsValidTarget(E.Range))
                            {
                                CastEPrediction(_enemyTarget);
                            }
                        }
                        if (RengarHasPassive && Ferocity == 5)
                        {
                            if (W.IsReady() && _enemyTarget.IsValidTarget(W.Range))
                            {
                                W.Cast();
                            }
                        }
                        if (_enemyTarget.IsValidTarget(E.Range) && E.IsReady() && MenuChecker.UseEoutofQRange && !RengarQ && !RengarHasPassive)
                        {
                            CastEPrediction(_enemyTarget);
                        }
                    }
                    break;
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        internal static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            try
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

            if (Ferocity == 5 && MenuChecker.ComboModeSelected == 1)
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
                        if (Misc.BetaQVariables.BetaQActive && TargetSelector.SelectedTarget.IsValidTarget(Misc.BetaQVariables.BetaQRange) && MenuChecker.ComboModeSelected == 1 && RengarHasUltimate && Q.IsReady())
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
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public static void CastItems()
        {
            try
            {
            if (!(Item.CanUseItem(ItemId.Ravenous_Hydra_Melee_Only) || Item.CanUseItem(ItemId.Tiamat_Melee_Only)))
            {
                return;
            }

            Item.UseItem(ItemId.Ravenous_Hydra_Melee_Only);
            Item.UseItem(ItemId.Tiamat_Melee_Only);


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void CastSmite(SpellSlot smiteSlotx, AIHeroClient target)
        {
            try
            {
                if (MenuChecker.ComboSmiteActive && !RengarHasUltimate && Misc.Smite != SpellSlot.Unknown
                    && Rengar.Spellbook.CanUseSpell(Misc.Smite) == SpellState.Ready && target.IsValidTarget(500))
                {
                    Rengar.Spellbook.CastSpell(smiteSlotx, target);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static void QPriority(AIHeroClient target)
        {
            try{
            var qPrioActive = Menu.ComboM["qprio.active"].Cast<CheckBox>().CurrentValue;
            if (!qPrioActive) { return; }
            if (RengarQ && !RengarHasPassive && !Q.IsOnCooldown)
            {
                if (target.IsValidTarget(Q.Range) && !(target.IsMinion || target.IsMonster)) { 
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }
            }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void CastEPrediction(AIHeroClient eCastTarget)
        {
            try
            {
            var predictionE = E.GetPrediction(eCastTarget);
            if (predictionE.HitChancePercent >= 88 && predictionE.CollisionObjects.Count() == 0 && !eCastTarget.IsMoving)
            {
                E.Cast(predictionE.CastPosition);
            }
            else if (predictionE.HitChancePercent >= 88 && predictionE.CollisionObjects.Count() == 0 &&
                     eCastTarget.IsMoving)
            {
                E.Cast(predictionE.UnitPosition);
            }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
