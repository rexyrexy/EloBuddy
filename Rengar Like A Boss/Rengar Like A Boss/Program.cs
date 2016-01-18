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

        private static void Skin()
        {
            var SkinHackActive = AllMenu["skin.active"].Cast<CheckBox>().CurrentValue;
            var SkinHackSelected = AllMenu["skin.value"].Cast<Slider>().CurrentValue;

            if(!SkinHackActive)
            {
                Rengar.SetSkinId(0);
                return;
            }

            switch (SkinHackSelected)
            {
                case 1: { Rengar.SetSkinId(1); break; }
                case 2: { Rengar.SetSkinId(2); break; }
                case 3: { Rengar.SetSkinId(3); break; }
            }
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

               if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear)) { LaneClear(); }

               if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)) { JungleClear(); }

                AutoHeal();

                AutoYoumu();
				
				Skin();
            }
        }

        private static void JungleClear()
        {
            var UseQActive = AllMenu["jungleclear.q"].Cast<CheckBox>().CurrentValue;
            var UseWActive = AllMenu["jungleclear.w"].Cast<CheckBox>().CurrentValue;
            var UseEActive = AllMenu["jungleclear.e"].Cast<CheckBox>().CurrentValue;
            var JungleClearSaveStacksActive = AllMenu["jungleclear.save"].Cast<CheckBox>().CurrentValue;
            foreach (var jungleMinion in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (Rengar.Mana < 5 || (Rengar.Mana == 5 && !JungleClearSaveStacksActive))
                {

                    if (UseQActive && Q.IsReady() && Rengar.Distance(jungleMinion) < Rengar.AttackRange)
                    {
                        Q.Cast();
                        Orbwalker.ResetAutoAttack();
                        Items();
                    }
                    if (UseWActive && W.IsReady() && Rengar.Distance(jungleMinion) <= W.Range)
                    {
                        W.Cast();
                    }
                    if (UseEActive && E.IsReady() && Rengar.Distance(jungleMinion) <= E.Range)
                    {
                        E.Cast(jungleMinion);
                    }
                }
            }
        }

        private static void LaneClear()
        {
            var UseQActive = AllMenu["laneclear.q"].Cast<CheckBox>().CurrentValue;
            var UseWActive = AllMenu["laneclear.w"].Cast<CheckBox>().CurrentValue;
            var UseEActive = AllMenu["laneclear.e"].Cast<CheckBox>().CurrentValue;
            var LaneClearSaveStacksActive = AllMenu["laneclear.save"].Cast<CheckBox>().CurrentValue;
            var LaneTarget = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => !x.IsDead && Q.IsInRange(x));

            if (Rengar.Mana < 5 || (Rengar.Mana == 5 && !LaneClearSaveStacksActive))
            {
                if (UseWActive && W.IsReady() && LaneTarget.IsValidTarget())
                {
                    W.Cast(LaneTarget);
                }
                if (UseQActive && Q.IsReady() && LaneTarget.IsValidTarget())
                {
                    Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
                if (LaneTarget.IsValidTarget(Rengar.GetAutoAttackRange()))
                {
                    Items();
                }
                if (UseEActive && E.IsReady() && LaneTarget.IsValidTarget())
                {
                    E.Cast(LaneTarget);
                }
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
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                        }

                        if (Rengar.Mana == 5 && !RengarHasPassive && !RengarUltiActive) //When Have 5 Prio Use Q
                        {
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                        }

                        if (RengarUltiActive && !RengarHasPassive && Q.IsReady() && UltiTarget.IsValidTarget(600)) { Q.Cast(); } //Cast Q Before Jump Target When Ulti

                        if (RengarHasPassive && Rengar.Mana <= 4) //Passive Logic
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                        }
                        if (RengarHasPassive && Rengar.Mana == 5)
                        {
                            if (Q.IsReady() && NormalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                        }
                        break;
                    }
                case 0://Snare Combo
                    {
                        if (Rengar.Mana <= 4 && !RengarHasPassive && !RengarUltiActive) //Normal Lane Target Logic
                        {
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                        }

                        if (Rengar.Mana == 5 && !RengarHasPassive && !RengarUltiActive) //When Have 5 Prio Use E
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                        }

                        if (RengarHasPassive && Rengar.Mana <= 4) //Passive Logic
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                            if (W.IsReady() && NormalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && NormalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(NormalTarget);
                        }
                        if (RengarHasPassive && Rengar.Mana == 5)
                        {
                            if (E.IsReady() && NormalTarget.IsValidTarget(E.Range) && EPrediction.HitChance >= HitChance.Medium) { E.Cast(NormalTarget); }
                        }
                        break;
                    }
            }
        }
        private static void MenuInit()
        {
            Menu = MainMenu.AddMenu("Rengar LAB", "_RengarLAB");
            Menu.AddGroupLabel("Rengar Like A Boss Loaded Suckssfully (No Subliminal)..");
            Menu.AddLabel("This great addon made by Rexy..");
            Menu.AddLabel("If you found a Bug, pls feedback it on my Thread..");
            Menu.AddLabel("I need your feedback for make this addon more cool..");
            Menu.AddLabel("My main aim for making this addon, i wanted oneshot addon for Rengod..");
            Menu.AddLabel("Have fun !");

            AllMenu = Menu.AddSubMenu("All Settingz", "_AllMenu");
            AllMenu.AddSeparator();
            AllMenu.AddGroupLabel("Combo Mode");
            AllMenu.AddLabel("| 1 -> One Shot || 2 -> Snare |");
            AllMenu.Add("combo.mode", new Slider("Combo Mode", 1, 1, 2));
            AllMenu.Add("autoyoumu", new CheckBox("Auto Youmu When Ulti"));
            AllMenu.Add("draw.mode", new CheckBox("Draw Mode"));
            AllMenu.AddSeparator();
            AllMenu.AddGroupLabel("LaneClear Mode");
            AllMenu.Add("laneclear.q", new CheckBox("Use Q"));
            AllMenu.Add("laneclear.w", new CheckBox("Use W"));
            AllMenu.Add("laneclear.e", new CheckBox("Use E"));
            AllMenu.Add("laneclear.save", new CheckBox("Save Stacks", false));
            AllMenu.AddSeparator();
            AllMenu.Add("jungleclear.q", new CheckBox("Use Q"));
            AllMenu.Add("jungleclear.w", new CheckBox("Use W"));
            AllMenu.Add("jungleclear.e", new CheckBox("Use E"));
            AllMenu.Add("jungleclear.save", new CheckBox("Save Stacks", false));
            AllMenu.AddSeparator();
            AllMenu.AddGroupLabel("Auto Hp %x when 5 prio");
            AllMenu.Add("autohp.active", new CheckBox("AutoHP Active"));
            AllMenu.Add("autohp.value", new Slider("AutoHP Value", 30, 1, 100));
            AllMenu.AddSeparator();
            AllMenu.Add("skin.active", new CheckBox("Skin Hack Active"));
            AllMenu.AddLabel("| 1 -> HeadHunter || 2 -> NighHunter || 3-> SSW");
            AllMenu.Add("skin.value", new Slider("Selected Skin", 3, 1, 3));
        }

        private static void Items()
        {
            if (Item.CanUseItem(3074)) { Item.UseItem(3074); }
            if (Item.CanUseItem(3077)) { Item.UseItem(3077); }
        }

        private static void BotrkAndBilgewater(AIHeroClient targetforuseBotRK)
        {
            if (Item.CanUseItem(3144)) { Item.UseItem(3144, targetforuseBotRK); }
            if (Item.CanUseItem(3153)) { Item.UseItem(3153, targetforuseBotRK); }
        }

        private static void AutoYoumu()
        {
            var AutoYoumuActive = AllMenu["autoyoumu"].Cast<CheckBox>().CurrentValue;
            
            if (!AutoYoumuActive) { return; }
            if (RengarUltiActive && Item.CanUseItem(3142)) { Item.UseItem(3142); }
        }
    }
}
