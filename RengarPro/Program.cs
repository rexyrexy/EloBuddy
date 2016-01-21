using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using Color = System.Drawing.Color;

namespace RengarPro
{
    class Program
    {
        public static AIHeroClient Rengar
        {
            get
            {
                return Player.Instance;
            }
        }
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static Menu Menu, AllMenu;

        public static bool RengarHasPassive
        {
            get
            {
                return Rengar.HasBuff("rengarpassivebuff");
            }
        }

        public static bool RengarUltiActive
        {
            get
            {
                return Rengar.HasBuff("RengarR");
            }
        }

        static void Main()
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (Rengar.Hero != Champion.Rengar)
            {
                return;
            }

            Q = new Spell.Active(SpellSlot.Q, 250);
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            MenuInit();
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            AutoHeal();
            AutoYoumu();
            Skin();
            BetaQ();
        }

        private static void BetaQ()
        {
            var betaQTarget = TargetSelector.GetTarget(2000, DamageType.Physical);
            var comboselecctedd = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
            if (RengarUltiActive && (int)Rengar.Mana == 5 && comboselecctedd == 1 && betaQTarget.Distance(Rengar.ServerPosition) <= 1000)
            {
                Core.DelayAction(null, 500);
                Q.Cast();
                Orbwalker.ResetAutoAttack();
            }
        }

        private static void Skin()
        {
            var skinHackActive = AllMenu["skin.active"].Cast<CheckBox>().CurrentValue;
            var skinHackSelected = AllMenu["skin.value"].Cast<Slider>().CurrentValue;

            if(!skinHackActive)
            {
                Rengar.SetSkinId(0);
                return;
            }

            switch (skinHackSelected)
            {
                case 1: { Rengar.SetSkinId(1); break; }
                case 2: { Rengar.SetSkinId(2); break; }
                case 3: { Rengar.SetSkinId(3); break; }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var comboModeDrawActive = AllMenu["draw.mode"].Cast<CheckBox>().CurrentValue;
            var comboModeSelected = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
           
            if(!comboModeDrawActive) { return; }

            switch (comboModeSelected)
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
        private static void JungleClear()
        {
            var useQActive = AllMenu["jungleclear.q"].Cast<CheckBox>().CurrentValue;
            var useWActive = AllMenu["jungleclear.w"].Cast<CheckBox>().CurrentValue;
            var useEActive = AllMenu["jungleclear.e"].Cast<CheckBox>().CurrentValue;
            var jungleClearSaveStacksActive = AllMenu["jungleclear.save"].Cast<CheckBox>().CurrentValue;
            foreach (var jungleMinion in EntityManager.MinionsAndMonsters.Monsters)
            {
                if (Rengar.Mana < 5 || ((int)Rengar.Mana == 5 && !jungleClearSaveStacksActive))
                {

                    if (useQActive && Q.IsReady() && Rengar.Distance(jungleMinion) < Rengar.AttackRange)
                    {
                        Q.Cast();
                        Orbwalker.ResetAutoAttack();
                        Items();
                    }
                    if (useWActive && W.IsReady() && Rengar.Distance(jungleMinion) <= W.Range)
                    {
                        W.Cast();
                    }
                    if (useEActive && E.IsReady() && Rengar.Distance(jungleMinion) <= E.Range)
                    {
                        E.Cast(jungleMinion);
                    }
                }
            }
        }

        private static void LaneClear()
        {
            var useQActive = AllMenu["laneclear.q"].Cast<CheckBox>().CurrentValue;
            var useWActive = AllMenu["laneclear.w"].Cast<CheckBox>().CurrentValue;
            var useEActive = AllMenu["laneclear.e"].Cast<CheckBox>().CurrentValue;
            var laneClearSaveStacksActive = AllMenu["laneclear.save"].Cast<CheckBox>().CurrentValue;
            var laneTarget = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(x => !x.IsDead && Q.IsInRange(x));

            if (Rengar.Mana < 5 || ((int)Rengar.Mana == 5 && !laneClearSaveStacksActive))
            {
                if (useWActive && W.IsReady() && laneTarget.IsValidTarget())
                {
                    W.Cast(laneTarget);
                }
                if (useQActive && Q.IsReady() && laneTarget.IsValidTarget())
                {
                    Q.Cast();
                    Orbwalker.ResetAutoAttack();
                }
                if (laneTarget.IsValidTarget(Rengar.GetAutoAttackRange()))
                {
                    Items();
                }
                if (useEActive && E.IsReady() && laneTarget.IsValidTarget())
                {
                    E.Cast(laneTarget);
                }
            }
        }

        private static void AutoHeal()
        {
            var tickedAutoHp = AllMenu["autohp.active"].Cast<CheckBox>().CurrentValue;
            var valueOfAutoHp = AllMenu["autohp.value"].Cast<Slider>().CurrentValue;

            if(tickedAutoHp && (int)Rengar.Mana == 5 && Rengar.HealthPercent <= valueOfAutoHp) { W.Cast(); }
        }

        private static void Combo()
        {
            var comboModeSelected = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
            var normalTarget = TargetSelector.GetTarget(1000, DamageType.Physical);
            var ePrediction = E.GetPrediction(normalTarget);
            var useEOutQRangeActive = AllMenu["useeoutofq"].Cast<CheckBox>().CurrentValue;

            if (RengarUltiActive)
            {
                return;
            }
            switch (comboModeSelected)
            {
                case 1://OneShot Mode Active
                    {
                        if (Rengar.Mana <= 4 && !RengarHasPassive) //Normal Lane Target Logic
                        {
                            if (W.IsReady() && normalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && normalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                            if (E.IsReady() && normalTarget.IsValidTarget(E.Range) && ePrediction.HitChance >= HitChance.Medium) { E.Cast(normalTarget); }
                        }

                        if ((int)Rengar.Mana == 5 && !RengarHasPassive) //When Have 5 Prio Use Q
                        {
                            if (Q.IsReady() && normalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                        }

                        if (RengarHasPassive && Rengar.Mana <= 4) //Passive Logic
                        {
                            if (Q.IsReady() && normalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                            if (W.IsReady() && normalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                        }
                        if (RengarHasPassive && (int)Rengar.Mana == 5)
                        {
                            if (Q.IsReady() && normalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                        }
                        if (!RengarHasPassive && normalTarget.Distance(Rengar.ServerPosition) <= E.Range &&
                            useEOutQRangeActive)//Use E out of Range Q When One Shot Mode Active
                        {
                            if (E.IsReady() && normalTarget.IsValidTarget(E.Range) &&
                                ePrediction.HitChance >= HitChance.Medium && !RengarHasPassive)
                            {
                                E.Cast(normalTarget);
                            }
                        }
                        break;
                    }
                case 2://Snare Combo
                    {
                        if (Rengar.Mana <= 4 && !RengarHasPassive) //Normal Lane Target Logic
                        {
                            if (W.IsReady() && normalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                            if (Q.IsReady() && normalTarget.IsValidTarget(Q.Range)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                            if (E.IsReady() && normalTarget.IsValidTarget(E.Range) && ePrediction.HitChance >= HitChance.Medium) { E.Cast(normalTarget); }
                        }

                        if ((int)Rengar.Mana == 5 && !RengarHasPassive) //When Have 5 Prio Use E
                        {
                            if (E.IsReady() && normalTarget.IsValidTarget(E.Range) && ePrediction.HitChance >= HitChance.Medium) { E.Cast(normalTarget); }
                        }

                        if (RengarHasPassive && Rengar.Mana <= 4) //Passive Logic
                        {
                            if (Q.IsReady() && normalTarget.IsValidTarget(600)) { Q.Cast(); Orbwalker.ResetAutoAttack(); }
                            Items();
                            BotrkAndBilgewater(normalTarget);
                            if (W.IsReady() && normalTarget.IsValidTarget(W.Range)) { W.Cast(); }
                        }
                        if (RengarHasPassive && (int)Rengar.Mana == 5)
                        {
                            if (E.IsReady() && normalTarget.IsValidTarget(E.Range) && ePrediction.HitChance >= HitChance.Medium) { E.Cast(normalTarget); }
                        }

                        break;

                    }
            }
        }
        private static void MenuInit()
        {
            Menu = MainMenu.AddMenu("RengarPro", "_RengarPro");
            Menu.AddGroupLabel("RengarPro Loaded Suckssfully (No Subliminal)..");
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
            var switcher = AllMenu.Add("Switcher", new KeyBind("Combo Mode Switcher", false, KeyBind.BindTypes.HoldActive, (uint)'T'));
            switcher.OnValueChange += delegate (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
            {
                if (args.NewValue == true)
                {
                    var cast = AllMenu["combo.mode"].Cast<Slider>();
                    if (cast.CurrentValue == cast.MaxValue)
                    {
                        cast.CurrentValue = 0;
                    }
                    else
                    {
                        cast.CurrentValue++;
                    }
                }
            };
            AllMenu.Add("autoyoumu", new CheckBox("Auto Youmu When Ulti"));
            AllMenu.Add("draw.mode", new CheckBox("Draw Mode"));
            AllMenu.Add("useeoutofq", new CheckBox("Use E out of Q Range"));
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
            AllMenu.Add("autohp.value", new Slider("AutoHP Value", 30, 1));
            AllMenu.AddSeparator();
            AllMenu.Add("skin.active", new CheckBox("Skin Hack Active"));
            AllMenu.AddLabel("| 1 -> HeadHunter || 2 -> NighHunter || 3-> SSW");
            AllMenu.Add("skin.value", new Slider("Selected Skin", 3, 1, 3));
        }

        private static void Items()
        {
            var normalTarget = TargetSelector.GetTarget(1000, DamageType.Physical);
            if (Item.CanUseItem(3074) && normalTarget.IsValidTarget(400))
            {
                Item.UseItem(3074);
            }
            if (Item.CanUseItem(3077) && normalTarget.IsValidTarget(400))
            {
                Item.UseItem(3077);
            }
        }

        private static void BotrkAndBilgewater(AIHeroClient targetforuseBotRk)
        {
            if (Item.CanUseItem(3144)) { Item.UseItem(3144, targetforuseBotRk); }
            if (Item.CanUseItem(3153)) { Item.UseItem(3153, targetforuseBotRk); }
        }

        private static void AutoYoumu()
        {
            var autoYoumuActive = AllMenu["autoyoumu"].Cast<CheckBox>().CurrentValue;
            
            if (!autoYoumuActive) { return; }
            if (RengarUltiActive && Item.CanUseItem(3142)) 
			{
            Core.DelayAction(null,800);
			Item.UseItem(3142); 
			}
        }
    }
}
