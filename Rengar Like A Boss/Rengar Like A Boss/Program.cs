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
        public static Obj_AI_Base SelectedEnemy;
        public static int LastAutoAttack, Lastrengarq;

        public static int LastQ, LastE, LastW, LastSpell;
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
            Dash.OnDash += OnDash;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Orbwalker.OnPreAttack += BeforeAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Game.OnWndProc += OnClick;
            Game.OnTick += Game_OnTick;
            MenuInit();
        }

        private static void OnClick(WndEventArgs args)
        {
            if (args.Msg != (uint)WindowMessages.LeftButtonDown)
            {
                return;
            }
            var unit2 =
                ObjectManager.Get<Obj_AI_Base>()
                    .FirstOrDefault(
                        a =>
                        (a.IsValidTarget()) && a.IsEnemy && a.Distance(Game.CursorPos) < a.BoundingRadius + 80
                        );
            if (unit2 != null)
            {
                SelectedEnemy = unit2;
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
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

        private static void BeforeAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            var options = AllMenu["combo.mode"].Cast<Slider>().CurrentValue;
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && !Rengar.HasBuff("rengarpassivebuff") && Q.IsReady() && options != 2 && Rengar.Mana == 5)
            {
                var x = Prediction.Position.PredictUnitPosition(target as Obj_AI_Base, (int)Rengar.AttackCastDelay + (int)0.04f);
                if (Rengar.Distance(x) <= Rengar.BoundingRadius + Rengar.AttackRange + target.BoundingRadius)
                {
                    args.Process = false;
                    Q.Cast();
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
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
                        if (Orbwalker.LastAutoAttack < Core.GameTickCount - Game.Ping / 2 && Core.GameTickCount - Game.Ping / 2 < Orbwalker.LastAutoAttack + Rengar.AttackCastDelay * 1000 + 40) Orbwalker.ResetAutoAttack();
                        break;

                    case "rengarw":
                        LastW = Environment.TickCount;
                        LastSpell = Environment.TickCount;
                        break;
                }
            }
        }

        private static void OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
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
                if (Rengar.Mana == 5)
                {
                    switch (AllMenu["combo.mode"].Cast<Slider>().CurrentValue)
                    {
                        case 2:
                            if (E.IsReady() && target.IsValidTarget(E.Range))
                            {
                                E.Cast(target);
                            }
                            break;
                        case 1:
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
                                    Items();
                                    BotrkAndBilgewater(target);
                                };
                            }

                            break;
                    }
                }
            }

            switch (AllMenu["combo.mode"].Cast<Slider>().CurrentValue)
            {
                case 2:
                    if (E.IsReady() && target.IsValidTarget(E.Range))
                    {
                        E.Cast(target);
                    }
                    break;

                case 1:
                    if (RengarUltiActive)
                    {
                        Q.Cast();
                    }
                    break;
            }

            if (e.Duration - 100 - Game.Ping / 2 > 0)
            {
                Core.DelayAction(null, (int)(e.Duration - 100 - Game.Ping / 2));
                { Items(); BotrkAndBilgewater(target); };


            }
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
            var SelectedEnemyDrawActive = AllMenu["draw.selectedenemy"].Cast<CheckBox>().CurrentValue;

            if(ComboModeDrawActive)
            {
                switch (ComboModeSelected)
                {
                    case 1:
                        {
                            Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : OneShot");
                            break;
                        }
                    case 2:
                        {
                            Drawing.DrawText(Drawing.Width * 0.70f, Drawing.Height * 0.95f, Color.White, "Mode : Snare");
                            break;
                        }
                }

                if (SelectedEnemyDrawActive && SelectedEnemy.IsValidTarget() && SelectedEnemy.IsVisible && !SelectedEnemy.IsDead)
                {
                    Drawing.DrawText(
                    Drawing.WorldToScreen(SelectedEnemy.Position).X - 40,
                    Drawing.WorldToScreen(SelectedEnemy.Position).Y + 10,
                    Color.White,
                    "Selected Target");
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
            try
            {
                var target = TargetSelector.GetTarget(R.Range, DamageType.Physical);

                if (SelectedEnemy.IsValidTarget(E.Range))
                {
                    TargetSelector.GetPriority(target);
                    if (TargetSelector.SelectedTarget != null)
                    {
                        TargetSelector.GetPriority(TargetSelector.SelectedTarget);
                    }
                }

                if (target.IsValidTarget(250))
				{
			        Items();
                    BotrkAndBilgewater(target);
				}

                if (Rengar.Mana <= 4)
                {
                    if (W.IsReady() && target.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }

                    if (Q.IsReady() && target.IsValidTarget(Q.Range))
                    {
                        Q.Cast();
                    }

                    if (!RengarHasPassive && E.IsReady())
                    {
                        if (target.IsValidTarget(E.Range) && !RengarUltiActive)
                        {
                            E.Cast(target);
                        }
                    }
                }

                if (Rengar.Mana == 5)
                {
                    switch (AllMenu["combo.mode"].Cast<Slider>().CurrentValue)
                    {
                        case 2:
                            if (!RengarUltiActive && target.IsValidTarget(E.Range) && E.IsReady())
                            {
                                var prediction = E.GetPrediction(target);
                                if (prediction.HitChance >= HitChance.High && prediction.CollisionObjects.Count() == 0)
                                {
                                    E.Cast(target.ServerPosition);
                                }
                            }
                            break;
                        case 1:
                            if (Q.IsReady()
                                && target.IsValidTarget(Q.Range))
                            {
                                Q.Cast();
                            }
                            break;
                    }

                    if (!RengarUltiActive)
                    {
                        if (AllMenu["eoutofq"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(E.Range))
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
            AllMenu.Add("eoutofq", new CheckBox("Use E out of Q Range"));
            AllMenu.Add("draw.mode", new CheckBox("Draw Mode"));
            AllMenu.Add("draw.selectedenemy", new CheckBox("Draw Selected Enemy"));
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
