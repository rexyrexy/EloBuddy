using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Utils;

namespace First_Class_Rengar
{
    public class ActiveModes : Standarts
    {
        public static Item Tiamat { get; private set; }
        public static Item Hydra { get; private set; }
        public static void Combo()
        {
            try
            {
                // ReSharper disable once ConvertConditionalTernaryToNullCoalescing
                var target = TargetSelector.SelectedTarget != null
                                 ? TargetSelector.SelectedTarget
                                 : TargetSelector.GetTarget(R.Range, DamageType.Physical);

                if (target == null)
                {
                    return;
                }

                if (Rengar.SelectedEnemy.IsValidTarget(E.Range))
                {
                    TargetSelector.GetPriority(target);
                    if (TargetSelector.SelectedTarget != null)
                    {
                        TargetSelector.GetPriority(TargetSelector.SelectedTarget);
                    }
                }

                CastItems(target);

                #region RengarR

                if (Ferocity <= 4)
                {
                    if (MenuInit.ComboMenu["Combo.Use.W"].Cast<CheckBox>().CurrentValue && W.IsReady())
                    {
                        CastW(target);
                    }

                    if (Q.IsReady() && MenuInit.ComboMenu["Combo.Use.Q"].Cast<CheckBox>().CurrentValue
                        && target.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }

                    if (!HasPassive && MenuInit.ComboMenu["Combo.Use.E"].Cast<CheckBox>().CurrentValue && E.IsReady())
                    {
                        if (target.IsValidTarget(E.Range) && !RengarR)
                        {
                            CastE(target);
                        }
                    }
                }

                if (Ferocity == 5)
                {
                    switch (MenuInit.ComboMenu["css"].Cast<Slider>().CurrentValue)
                    {
                        case 0:
                            if (!RengarR && target.IsValidTarget(E.Range) && E.IsReady())
                            {
                                var prediction = E.GetPrediction(target);
                                if (prediction.HitChance >= HitChance.High && prediction.CollisionObjects.Count() == 0)
                                {
                                    E.Cast(target.ServerPosition);
                                }
                            }
                            break;
                        case 1:
                            if (MenuInit.ComboMenu["Combo.Use.W"].Cast<CheckBox>().CurrentValue && W.IsReady()
                                && target.IsValidTarget(W.Range) && !HasPassive)
                            {
                                CastW(target);
                            }
                            break;
                        case 2:
                            if (MenuInit.ComboMenu["Combo.Use.Q"].Cast<CheckBox>().CurrentValue && Q.IsReady()
                                && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                            }
                            break;
                    }

                    if (!RengarR)
                    {
                        if (MenuInit.ComboMenu["Combo.Use.E.OutOfRange"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(E.Range))
                        {
                            var prediction = E.GetPrediction(target);
                            if (prediction.HitChance >= HitChance.Dashing && prediction.CollisionObjects.Count() == 0)
                            {
                                E.Cast(target);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }

        #endregion

        #region Methods

        private static void CastE(Obj_AI_Base target)
        {
            if (!E.IsReady() || !target.IsValidTarget(E.Range))
            {
                return;
            }

            var prediction = E.GetPrediction(target);

            if (prediction.HitChance >= HitChance.High)
            {
                E.Cast(target.ServerPosition);
            }
        }

        private static void CastW(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(W.Range) || !W.IsReady())
            {
                return;
            }
            W.Cast();
        }

        #endregion

        public static void CastItems(Obj_AI_Base target)
        {

            Tiamat = new Item(3077, 400f);
            Hydra = new Item(3074, 400f);

            if (target.IsValidTarget(400f))
            {
                if (Tiamat.IsReady())
                {
                    Tiamat.Cast();
                }

                if (Hydra.IsReady())
                {
                    Hydra.Cast();
                }
            }
        }
    }
}
