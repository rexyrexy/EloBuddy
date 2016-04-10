using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using RyzePro.Modes;

namespace RyzePro
{
    class Program
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        readonly static Random Seeder = new Random();
        static float gametime;
        static float delay;
        static float startTime;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Starting.Ryze.Hero != Champion.Ryze)
            {
                return;
            }
            Menu.Init();
            Game.OnUpdate += Game_OnUpdate;
            Gapcloser.OnGapcloser += Gapcloser_OnGapcloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Chat.Print("RyzePro | Loaded..",Color.DarkGreen);
            Chat.Print("RyzePro | Coded by Rexy",Color.Blue);
            Chat.Print("RyzePro | Get rekt from Ryze",Color.Chocolate);
        }

        static void Game_OnUpdate(EventArgs args)
        {
            gametime = (Game.Time - startTime) * 1000;
            if ((gametime >= delay && Checker.HumanizerActive) || !Checker.HumanizerActive)
            {
                gametime = 0;
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    Modes.Combo.Do();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
                {
                    Modes.Harass.Do();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
                {
                    Modes.LaneClear.Do();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
                {
                    Modes.JungleClear.Do();
                }
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
                {
                    Modes.LastHit.Do();
                }
                startTime = Game.Time;
                delay = Seeder.Next(Checker.HumanizerMinValue, Checker.HumanizerMaxValue);
            }
        }

        static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (Checker.AntiGapCloser && sender.IsEnemy && Spells.W.IsReady()
                && sender.Distance(Ryze.ServerPosition) < Spells.W.Range)
            {
                Spells.W.Cast(sender);
            }
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Checker.WhileComboAa && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && (Combo.QReady || Combo.EReady || Combo.WReady))
            {
                args.Process = false;
            }
            else if (Checker.WhileComboAa && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && !(Combo.QReady || Combo.EReady || Combo.WReady))
            {
                args.Process = true;
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Ryze.IsDead || Checker.DrawNo) { return; }

            if (Checker.DrawQ) { Drawing.DrawCircle(Ryze.Position, Spells.Q.Range, Color.White); }
            if (Checker.DrawW) { Drawing.DrawCircle(Ryze.Position, Spells.W.Range, Color.Blue); }
            if (Checker.DrawE) { Drawing.DrawCircle(Ryze.Position, Spells.E.Range, Color.Brown); }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Checker.AntiGapCloser && sender.IsEnemy && Spells.W.IsReady()
                && sender.Distance(Ryze.ServerPosition) < Spells.W.Range)
            {
                Spells.W.Cast(sender);
            }
        }
    }
}