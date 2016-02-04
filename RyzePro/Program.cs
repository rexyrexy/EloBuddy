using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;

namespace RyzePro
{
    class Program
    {
        public static AIHeroClient Ryze = Starting.Ryze;
        readonly static Random Seeder = new Random();
        static float gametime;
        static float delay;
        static float startTime;
        public static int PassiveStack = Starting.StackPassive;
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
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
        }

        private static void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (Checker.WhileComboAa && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                args.Process = false;
            }
            else
            {
                if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
                {
                    args.Process = !(Spells.Q.IsReady() || Spells.W.IsReady() || Spells.E.IsReady()
                          || Ryze.Distance(args.Target) >= 1000);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Ryze.IsDead || Checker.DrawNo) { return; }

            if (Checker.DrawQ) { Drawing.DrawCircle(Ryze.Position, Spells.Q.Range, System.Drawing.Color.White); }
            if (Checker.DrawW) { Drawing.DrawCircle(Ryze.Position, Spells.W.Range, System.Drawing.Color.Blue); }
            if (Checker.DrawE) { Drawing.DrawCircle(Ryze.Position, Spells.E.Range, System.Drawing.Color.Brown); }
        }

        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (Checker.AntiGapCloser && Spells.W.IsReady()
                && sender.Distance(Ryze.ServerPosition) < Spells.W.Range)
            {
                Spells.W.Cast(sender);
            }
        }

        private static void Game_OnUpdate(EventArgs args)
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
    }
}