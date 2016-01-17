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
using Color = System.Drawing.Color;

namespace First_Class_Rengar
{
    internal class Rengar
    {
        public static Menu MainMenu;
        public static Menu ComboMenu, ClearMenu, MainnMenu, JungleClear, Heal, KillSteal, BetaMenu, MiscMenu;
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static AIHeroClient Me { get { return ObjectManager.Player; } }

        public static int LastAutoAttack, Lastrengarq;

        public static int LastQ, LastE, LastW, LastSpell;

        public static Obj_AI_Base SelectedEnemy;

        public static int Ferocity
        {
            get
            {
                return (int)Me.Mana;
            }
        }
        public static bool HasPassive
        {
            get
            {
                return Me.HasBuff("rengarpassivebuff");
            }
        }
        protected static bool RengarR
        {
            get
            {
                return Me.Buffs.Any(x => x.Name.Contains("RengarR"));
            }
        }

        private static IEnumerable<AIHeroClient> Enemies
        {
            get
            {
                return EntityManager.Heroes.Enemies;
            }
        }
        public static int LastSwitch;
        public static void Spelll()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)(Me.GetAutoAttackRange(Me) + 100));
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 70);
            R = new Spell.Active(SpellSlot.R, 2000);
        }

        public static Item Tiamat { get; private set; }
        public static Item Hydra { get; private set; }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Rengar) { return; }
            MenuInit.Initialize();
            Game.OnTick += Game_OnTick;
            KillstealHandler();
            Dash.OnDash += OnDash;
            Drawing.OnDraw += OnDraw;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Orbwalker.OnPreAttack += BeforeAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Game.OnWndProc += OnClick;
            Chat.Print("First Class Loaded..");
            Chat.Print("#1 Rengar Script..");
            Chat.Print("Best Port From L$ Kappa");
            Chat.Print("Have Fun :) :) :)");
        }

		public static void OnClick(WndEventArgs args)
        {
            if (args.Msg != (uint)WindowMessages.LeftButtonDown)
            {
                return;
            }
            var unit2 =
                ObjectManager.Get<Obj_AI_Base>()
                    .FirstOrDefault(
                        a =>
                        (a.IsValid()) && a.IsEnemy && a.Distance(Game.CursorPos) < a.BoundingRadius + 80
                        );
            if (unit2 != null)
            {
                SelectedEnemy = unit2;
            }
        }
		
        static void Game_OnTick(EventArgs args)
        {
            Heall();

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                ComboXxX();
            }

        }

        static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (target.IsMe && Q.IsReady() && target is AIHeroClient
                    && target.IsValidTarget(Q.Range) && target.IsEnemy)
                {
                    Q.Cast();
                }
            }
        }

        static void ComboXxX()
        {
            try
            {
                // ReSharper disable once ConvertConditionalTernaryToNullCoalescing
                var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);

                if (Rengar.SelectedEnemy.IsValidTarget(E.Range))
                {
                    TargetSelector.GetPriority(target);
                    if (TargetSelector.SelectedTarget != null)
                    {
                        TargetSelector.GetPriority(TargetSelector.SelectedTarget);
                    }
                }

                CastItems(target);

              

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

      

        

        static void CastE(Obj_AI_Base target)
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

        static void CastW(Obj_AI_Base target)
        {
            if (!target.IsValidTarget(W.Range) || !W.IsReady())
            {
                return;
            }
            W.Cast();
        }

      

        static void CastItems(Obj_AI_Base target)
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

        static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe)
            {
                switch (args.SData.Name.ToLower())
                {
                    case "rengarr":
                        if (Item.CanUseItem(3142))
                            Core.DelayAction(null, 2000);
                            Item.UseItem(3142);
                        break;
                    case "rengarq":
                        LastQ = Environment.TickCount;
                        LastSpell = Environment.TickCount;
                        Orbwalker.ResetAutoAttack();
                        break;

                    case "rengare":
                        LastE = Environment.TickCount;
                        LastSpell = Environment.TickCount;
                        if (Orbwalker.LastAutoAttack < Core.GameTickCount - Game.Ping / 2 && Core.GameTickCount - Game.Ping / 2 < Orbwalker.LastAutoAttack + Me.AttackCastDelay * 1000 + 40) Orbwalker.ResetAutoAttack();
                        break;

                    case "rengarw":
                        LastW = Environment.TickCount;
                        LastSpell = Environment.TickCount;
                        break;
                }
            }
        }

        static void BeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            var options = MenuInit.ComboMenu["css"].DisplayName;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && !Me.HasBuff("rengarpassivebuff") && Q.IsReady() && options != "E" | options != "W" && Me.Mana == 5)
            {
                var x = Prediction.Position.PredictUnitPosition(target as Obj_AI_Base, (int)Me.AttackCastDelay + (int)0.04f);
                if (Me.Distance(x) <= Me.BoundingRadius + Me.AttackRange + target.BoundingRadius)
                {
                    args.Process = false;
                    Q.Cast();
                }
            }
        }

        static void Heall()
        {
            var hpactive = Heal["Heal.AutoHeal"].Cast<CheckBox>().CurrentValue;
            var hpslider = Heal["Heal.HP"].Cast<Slider>().CurrentValue;

            if (Me.IsRecalling() || Me.IsInShopRange() || Ferocity <= 4 || RengarR || Me.IsDead)
            {
                return;
            }

            if (hpactive
                && (Me.Health / Me.MaxHealth) * 100 <= hpslider
                && W.IsReady())
            {
                W.Cast();
            }
        }

        static void KillstealHandler()
        {
            if (!KillSteal["Killsteal.On"].Cast<CheckBox>().CurrentValue || Me.IsDead)
            {
                return;
            }

            var target =
                Enemies.FirstOrDefault(
                    x => x.IsValidTarget(W.Range) && x.Health < Me.GetSpellDamage(x, SpellSlot.W));
            if (target != null)
            {
                W.Cast();
            }
        }

        static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs args)
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

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                if (Ferocity == 5)
                {
                    switch (MenuInit.ComboMenu["css"].Cast<Slider>().CurrentValue)
                    {
                        case 0:
                            if (E.IsReady() && target.IsValidTarget(E.Range))
                            {
                                E.Cast(target);
                            }
                            break;
                        case 2:
                            if (Q.IsReady() && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                            }

                            if (target.IsValidTarget(Q.Range))
                            {
                                Core.DelayAction(null, 50);
                                {
                                    if (target.IsValidTarget(W.Range))
                                    {
                                        W.Cast();
                                    }

                                    E.Cast(target);
                                    CastItems(target);
                                };
                            }

                            break;
                    }
                }
            }

            switch (MenuInit.ComboMenu["css"].Cast<Slider>().CurrentValue)
            {
                case 0:
                    if (E.IsReady() && target.IsValidTarget(E.Range))
                    {
                        E.Cast(target);
                    }
                    break;

                case 2:
                    if (BetaMenu["Beta.Cast.Q"].Cast<CheckBox>().CurrentValue && RengarR)
                    {
                        Q.Cast();
                    }
                    break;
            }

            if (args.Duration - 100 - Game.Ping / 2 > 0)
            {
                Core.DelayAction(null, (int)(args.Duration - 100 - Game.Ping / 2));
                { CastItems(target); };


            }
        }
        static void OnDraw(EventArgs args)
        {
            var drawW = MenuInit.MiscMenu["Misc.Drawings.W"].Cast<CheckBox>().CurrentValue;
            var drawE = MenuInit.MiscMenu["Misc.Drawings.E"].Cast<CheckBox>().CurrentValue;

            if (Me.IsDead)
            {
                return;
            }


            if (SelectedEnemy.IsValidTarget() && SelectedEnemy.IsVisible && !SelectedEnemy.IsDead)
            {
                Drawing.DrawText(
                    Drawing.WorldToScreen(SelectedEnemy.Position).X - 40,
                    Drawing.WorldToScreen(SelectedEnemy.Position).Y + 10,
                    Color.White,
                    "Selected Target");
            }

            if (MenuInit.MiscMenu["Misc.Drawings.Off"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (drawW)
            {
                if (W.Level > 0)
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, W.Range, Color.Purple);
                }
            }

            if (drawE)
            {
                if (E.Level > 0)
                {
                    Drawing.DrawCircle(ObjectManager.Player.Position, E.Range, Color.White);
                }
            }

            if (MenuInit.MiscMenu["Misc.Drawings.Prioritized"].Cast<CheckBox>().CurrentValue)
            {
                switch (MenuInit.ComboMenu["css"].Cast<Slider>().CurrentValue)
                {
                    case 0:
                        Drawing.DrawText(
                            Drawing.Width * 0.70f,
                            Drawing.Height * 0.95f,
                            Color.Yellow,
                            "Prioritized spell: E");
                        break;
                    case 1:
                        Drawing.DrawText(
                            Drawing.Width * 0.70f,
                            Drawing.Height * 0.95f,
                            Color.White,
                            "Prioritized spell: W");
                        break;
                    case 2:
                        Drawing.DrawText(
                            Drawing.Width * 0.70f,
                            Drawing.Height * 0.95f,
                            Color.White,
                            "Prioritized spell: Q");
                        break;
                }
            }
        }
    }
}

   


    

