using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.Constants;
using SharpDX;
using System.Drawing;

namespace RebornRengar
{
    class Program
    {
        public static Menu ComboMenu, MainnMenu;
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static AIHeroClient Me { get { return ObjectManager.Player; } }

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Player.Instance.Hero != Champion.Rengar)
                return;

            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 70);
            R = new Spell.Active(SpellSlot.R);

            MainnMenu = MainMenu.AddMenu("Reborn Rengar", "rebornrengar");
            MainnMenu.AddGroupLabel("Reborn Rengar");
            MainnMenu.AddLabel("Coded by Rexy");

            ComboMenu = MainnMenu.AddSubMenu("Combo Menu", "combomenuSET");
            ComboMenu.Add("AutoWHP", new Slider("% Health", 30));
            var cs = ComboMenu.Add("css", new Slider("Combo Selector", 0, 0, 3));
            var co = new[] { "Burst", "Snare", "Always Q", "Auto" };
            cs.DisplayName = co[cs.CurrentValue];
            cs.OnValueChange +=
                delegate (ValueBase<int> sender, ValueBase<int>.ValueChangeArgs changeArgs)
                {
                    sender.DisplayName = co[changeArgs.NewValue];
                };

            Game.OnUpdate += Game_OnUpdate;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Obj_AI_Base.OnBuffLose += Obj_AI_Base_OnBuffLose;
            Obj_AI_Base.OnSpellCast += Obj_AI_Base_OnSpellCast;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!target.IsMe)
                return;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                var options = ComboMenu["css"].DisplayName;
                if (Q.IsReady())
                {
                    Q.Cast();
                }
                else if (E.IsReady())
                {
                    var targetE = TS.GetTarget(E.Range,DamageType.Physical);
                    if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                    {
                        E.Cast(targetE);
                    }
                    foreach (var tar in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                    {
                        if (E.IsReady())
                            E.Cast(tar);
                    }
                }
            }
        }

        private static void Obj_AI_Base_OnSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var spell = args.SData;
            if (!sender.IsMe)
                return;
            if (spell.Name.ToLower().Contains("rengarq"))
            {
                Orbwalker.ResetAutoAttack();
            }
            if (spell.Name.ToLower().Contains("rengare"))
                if (Orbwalker.LastAutoAttack < Game.TicksPerSecond - Game.Ping / 2 && Game.TicksPerSecond - Game.Ping / 2 < Orbwalker.LastAutoAttack + Me.AttackCastDelay * 1000 + 40)
                {
                    Orbwalker.ResetAutoAttack();
                }
        }

        private static void Obj_AI_Base_OnBuffLose(Obj_AI_Base sender, Obj_AI_BaseBuffLoseEventArgs args)
        {
            if (!sender.IsMe)
                return;
            if (args.Buff.Name == "rengarqbase" || args.Buff.Name == "rengarqemp")
            {

            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (Me.Mana == 5 && E.IsReady())
            {
                if (sender.IsValidTarget(E.Range))
                {
                    E.Cast(sender);
                }
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            var options = ComboMenu["css"].DisplayName;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo && !Me.HasBuff("rengarpassivebuff") && Q.IsReady() && options != "Snare" && Me.Mana == 5)
            {
                var x = Prediction.Position.PredictUnitPosition(target as Obj_AI_Base, (int) Me.AttackCastDelay + (int) 0.04f);
                if (Me.Distance(x) <= Me.BoundingRadius + Me.AttackRange + target.BoundingRadius)
                {
                    args.Process = false;
                    Q.Cast();
                }
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            switch (Orbwalker.ActiveModesFlags)
            {
                case Orbwalker.ActiveModes.Combo:
                    ComboX();
                    break;
            }

            var hpp = ComboMenu["AutoWHP"].Cast<Slider>().CurrentValue;
            if ((Me.Health / Me.MaxHealth) * 100 <= hpp && Me.Mana == 5 && W.IsReady())
            {
                W.Cast();
            }
        }
        private static void ComboX()
        {
            var options = ComboMenu["css"].DisplayName;
            switch (options)
            {
                case "Burst":
                    if (Me.Mana < 5)
                    {
                        var targetW = TS.GetTarget(500, DamageType.Physical);
                        if (W.IsReady() && targetW.IsValidTarget() && !targetW.IsZombie)
                        {
                            W.Cast(targetW);
                        }
                        if (Orbwalker.CanMove)
                        {
                            var targetE = TS.GetTarget(E.Range, DamageType.Physical);
                            if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                            {
                                E.Cast(targetE);
                            }
                            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                            {
                                if (E.IsReady())
                                    E.Cast(target);
                            }
                        }
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack /*&& dontwaitQ*/)
                            {
                                Q.Cast();
                            }
                        }
                    }
                    break;
                case "Snare":
                    if (Me.Mana < 5)
                    {
                        var targetW = TargetSelector.GetTarget(500, DamageType.Physical);
                        if (W.IsReady() && targetW.IsValidTarget() && !targetW.IsZombie)
                        {
                            W.Cast(targetW);
                        }
                        if (Orbwalker.CanMove)
                        {
                            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                            if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                            {
                                E.Cast(targetE);
                            }
                            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                            {
                                if (E.IsReady())
                                    E.Cast(target);
                            }
                        }
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack)
                            {
                                Q.Cast();
                            }
                        }
                    }
                    else
                    {
                        var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                        if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                        {
                            E.Cast(targetE);
                        }
                        foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                        {
                            if (E.IsReady())
                                E.Cast(target);
                        }
                    }
            break;
                case "Always Q":
                    if (Me.Mana < 5)
                    {
                        var targetW = TargetSelector.GetTarget(500, DamageType.Physical);
                        if (W.IsReady() && targetW.IsValidTarget() && !targetW.IsZombie)
                        {
                            W.Cast(targetW);
                        }
                        if (Orbwalker.CanMove)
                        {
                            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                            if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                            {
                                E.Cast(targetE);
                            }
                            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                            {
                                if (E.IsReady())
                                    E.Cast(target);
                            }
                        }
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack /*&& dontwaitQ*/)
                            {
                                Q.Cast();
                            }
                        }
                    }
                    else
                    {
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack)
                            {
                                Q.Cast();
                            }
                        }
                        if (Q.IsReady() && Me.IsDashing())
                        {
                            Q.Cast();
                        }
                    }
                    break;
                case "Auto":
                    if (Me.Mana < 5)
                    {
                        var targetW = TargetSelector.GetTarget(500, DamageType.Physical);
                        if (W.IsReady() && targetW.IsValidTarget() && !targetW.IsZombie)
                        {
                            W.Cast(targetW);
                        }
                        if (Orbwalker.CanMove)
                        {
                            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                            if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                            {
                                E.Cast(targetE);
                            }
                            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                            {
                                if (E.IsReady())
                                    E.Cast(target);
                            }
                        }
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack) 
                            {
                                Q.Cast();
                            }
                        }
                    }
                    else
                    {
                        if (Q.IsReady() && Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) != 0)
                        {
                            if (Orbwalker.CanMove && !Orbwalker.CanAutoAttack)
                            {
                                Q.Cast();
                            }

                        }
                        if (Me.CountEnemiesInRange(Me.AttackRange + Me.BoundingRadius + 100) == 0 && !Player.HasBuff("rengarpassivebuff"))
                        {
                            var targetE = TargetSelector.GetTarget(E.Range, DamageType.Physical);
                            if (E.IsReady() && targetE.IsValidTarget() && !targetE.IsZombie)
                            {
                                E.Cast(targetE);
                            }
                            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
                            {
                                if (E.IsReady())
                                    E.Cast(target);
                            }
                        }
                    }
                    break;
            }
        }
    }
}