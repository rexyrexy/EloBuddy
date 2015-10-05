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
                            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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
                            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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
                        foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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
                            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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
                            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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
                            foreach (var target in HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && !x.IsZombie))
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