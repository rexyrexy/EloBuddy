using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.Networking;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using SharpDX;

using Color = System.Drawing.Color;

namespace Rengar_Like_A_Boss
{
    class Program
    {
        public static AIHeroClient Rengar
        {get { return Player.Instance; } }
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static Menu Menu, AllMenu;
        public static bool RengarHasPassive { get { return Rengar.HasBuff("rengarpassivebuff"); } }
        public static bool RengarUltiActive { get { return Rengar.HasBuff("RengarR"); } }
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if(Rengar.Hero != Champion.Rengar) { return; }
            Bootstrap.Init(null);
            Q = new Spell.Active(SpellSlot.Q, 250);
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnTick += Game_OnTick;
            MenuInit();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var ComboModeDrawActive = AllMenu["draw.mode"].Cast<CheckBox>().CurrentValue;
            var ComboModeSelected = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
           
            if(!ComboModeDrawActive) { return; }

            switch (ComboModeSelected)
            {
                case 1:
                    {
                        Drawing.DrawText(Drawing.Width * 0.70f,Drawing.Height * 0.95f,Color.White,"Mode : OneShot");
                        break;
                    }
                case 2:
                    {
                        Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : Snare");
                        break;
                    }


            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            {
               if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo)) { Combo(); }

                AutoHeal();
            }
        }

        private static void AutoHeal()
        {
            var TickedAutoHp = AllMenu["autohp.active"].Cast<CheckBox>().CurrentValue;
            var ValueOfAutoHp = AllMenu["autohp.value"].Cast<Slider>().CurrentValue;

            if(TickedAutoHp && Rengar.Mana == 5 && Rengar.HealthPercent <= ValueOfAutoHp) { W.Cast(); }
        }

        private static void Combo()
        {
            var ComboModeSelected = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
            var UltiTarget = TargetSelector.GetTarget(2000, DamageType.Physical);
            var NormalTarget = TargetSelector.GetTarget(1000, DamageType.Physical);
            var EPrediction = E.GetPrediction(NormalTarget);

            switch (ComboModeSelected)
            {
                case 1://OneShot Mode Active
                    {
                        if (Rengar.Mana <= 4 && !RengarHasPassive && !RengarUltiActive) //Normal Lane Target Logic
                        {
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range) && Orbwalker.CanAutoAttack && !Orbwalker.IsAutoAttacking) { Q.Cast(); }
                            if (!Q.IsReady()) { Orbwalker.ResetAutoAttack(); }
                            Items();
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }
                        }

                        if (Rengar.Mana == 5 && !RengarHasPassive && !RengarUltiActive) //When Have 5 Prio Use Q
                        {
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range) && Orbwalker.CanAutoAttack && !Orbwalker.IsAutoAttacking) { Q.Cast(); }
                            Orbwalker.ResetAutoAttack();
                            Items();
                        }

                        if (RengarUltiActive && !RengarHasPassive && Q.IsReady() && UltiTarget.IsValidTarget(600)) { Q.Cast(); } //Cast Q Before Jump Target When Ulti

                        if (RengarHasPassive) //Passive Logic
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(600)) { Q.Cast(); }
                            Items();
                            if (!Q.IsReady()) { Orbwalker.ResetAutoAttack(); }
                        }
                        break;
                    }
                case 0://Snare Combo
                    {
                        if (Rengar.Mana <= 4 && !RengarHasPassive && !RengarUltiActive) //Normal Lane Target Logic
                        {
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range) && Orbwalker.CanAutoAttack && !Orbwalker.IsAutoAttacking) { Q.Cast(); }
                            if (!Q.IsReady()) { Orbwalker.ResetAutoAttack(); }
                            Items();
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }
                        }

                        if (Rengar.Mana == 5 && !RengarHasPassive && !RengarUltiActive) //When Have 5 Prio Use E
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }
                        }

                        if (RengarHasPassive && Rengar.Mana <= 4) //Passive Logic
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(600)) { Q.Cast(); }
                            Items();
                            if (!Q.IsReady()) { Orbwalker.ResetAutoAttack(); }
                        }
                        if(RengarHasPassive && Rengar.Mana == 5) { if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); }if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.High && EPrediction.CollisionObjects.Count() == 0) { E.Cast(EPrediction.CastPosition); } }
                        break;
                    }


            }
        }
        private static void MenuInit()
        {
            Menu = MainMenu.AddMenu("Rengar LAB", "_RengarLAB");
            Menu.AddGroupLabel("Rengar Like A Boss Loaded Suckssfully (No Subliminal)..");
            Menu.AddLabel("This great addon made by Rexy..");
            Menu.AddLabel("If you found an Bug, pls feedback it on my Thread..");
            Menu.AddLabel("I need your feedback for make this addon more cool..");
            Menu.AddLabel("My main aim for making this addon, i wanted oneshot addon for Rengod..");
            Menu.AddLabel("Have fun !");

            AllMenu = Menu.AddSubMenu("All Settingz", "_AllMenu");
            AllMenu.AddSeparator();
            AllMenu.AddGroupLabel("Combo Mode");
            AllMenu.AddLabel("| 1 -> One Shot || 2 -> Snare |");
            AllMenu.Add("combo.mode", new Slider("Combo Mode", 1, 1, 2));
            AllMenu.AddSeparator();
            AllMenu.AddGroupLabel("Auto Hp %x when 5 prio");
            AllMenu.Add("autohp.active", new CheckBox("AutoHP Active"));
            AllMenu.Add("autohp.value", new Slider("AutoHP Value", 30, 1, 100));
            AllMenu.AddSeparator();
            AllMenu.Add("draw.mode", new CheckBox("Draw Mode"));
        }

        private static void Items()
        {
            if (Item.CanUseItem(3074)) { Item.UseItem(3074); }
            if (Item.CanUseItem(3077)) { Item.UseItem(3077); }
        }
    }
}
