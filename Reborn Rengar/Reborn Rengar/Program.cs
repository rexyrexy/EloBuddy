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
            var Target = TS.GetTarget(E.Range, DamageType.Physical);
            
            if (Target.IsValidTarget(Q.Range) && Me.Mana < 5)
            {
                if (Q.IsReady())
                {
                    Q.Cast();
                }

                if (E.IsReady() && !Q.IsReady())
                {
                    E.Cast(Target);
                }

                if (W.IsReady() && !E.IsReady())
                {
                    W.Cast();
                }

                if (Q.IsReady() && !W.IsReady())
                {
                    Q.Cast();
                }

                if (E.IsReady() && !Q.IsReady())
                {
                    E.Cast(Target);
                }
            }
             else if (!Target.IsValidTarget(Q.Range) && Me.Mana < 5)
            {
                if (E.IsReady())
                {
                    E.Cast(Target);
                }
            }

            else if (Target.IsValidTarget(Q.Range) && Me.Mana == 5)
            {
                if (Q.IsReady())
                {
                    Q.Cast();
                }
            }

            else if (!Target.IsValidTarget(Q.Range) && Me.Mana == 5)
            {
                if (E.IsReady())
                {
                    E.Cast(Target);
                }
            }
        }
    }
}