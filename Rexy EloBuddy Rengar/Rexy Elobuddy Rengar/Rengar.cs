using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Rendering;
using SharpDX;
using Color = System.Drawing.Color;
using Item = Rexy_Elobuddy_Rengar.Activator.Item;

namespace Rexy_Elobuddy_Rengar
{
    using Rexy_Elobuddy_Rengar.Activator;

    using Item = EloBuddy.SDK.Item;

    internal class Rengar
    {
        public static Menu RengarMenu, ComboMenu, MiscMenu;

        public static Spell.Active Q;

        public static Spell.Active W;

        public static Spell.Skillshot E;

        public static Spell.Active R;

        public static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }



        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Hacks.AntiAFK = true;
            Bootstrap.Init(null);
            TargetSelector.Init();
            ItemManager.Init();
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 70);
            R = new Spell.Active(SpellSlot.R);

            RengarMenu = MainMenu.AddMenu("Rexy Rengar", "RexyRengarMenu");
            RengarMenu.AddGroupLabel("Rexy Rengar Addon");
            RengarMenu.AddSeparator();
            RengarMenu.AddLabel("Coded by Rexy");
            RengarMenu.AddLabel("www.elobuddy.net");

            ComboMenu = RengarMenu.AddSubMenu("Combo Settings", "ComboMenu");
            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Rengar.EoutRange", new CheckBox("E out of Range"));
            ComboMenu.AddSeparator();
            ComboMenu.AddLabel("On Early Game Tick E out Of Range , At Last Game Untick E out of Range For Oneshot");

            MiscMenu = RengarMenu.AddSubMenu("Misc Settings", "Misc");
            MiscMenu.AddLabel("Auto Health When %Hp <= X");
            MiscMenu.Add("Rengar.AutoHP", new Slider("Auto Health %HP", 30, 0, 100));
            MiscMenu.AddLabel("Anti GapCloser With Empowered E");
            MiscMenu.Add("Rengar.AntiGapCloser", new CheckBox("Use Empowered E On Gapcloser"));

            Game.OnTick += Game_OnTick;
            Gapcloser.OnGapCloser += Gapcloser_OnGapCloser;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapCloserEventArgs e)
        {
            var useE = MiscMenu["Rengar.AntiGapCloser"].Cast<CheckBox>().CurrentValue;
            var epred = E.GetPrediction(sender).HitChance;
            if (useE && E.IsReady() && epred >= HitChance.High && Player.Mana == 5 && sender.IsEnemy
                && e.End.Distance(Player) < E.Range)
            {
                E.Cast(sender);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.IsDead) return;
            if (Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo)
            {
                Combo();
            }

            var hp = MiscMenu["Rengar.AutoHP"].Cast<Slider>().CurrentValue;
            if ((Player.Health / Player.MaxHealth) * 100 <= hp && Player.Mana == 5 && W.IsReady())
            {
                W.Cast();
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (
                var enemyVisible in ObjectManager.Get<Obj_AI_Base>().Where(enemyVisible => enemyVisible.IsValidTarget())
                )
            {
                if (ComboDamage(enemyVisible) > enemyVisible.Health)
                {
                    Drawing.DrawText(
                        Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                        Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                        Color.Red,
                        "One Shot");
                }
                else if (ComboDamage(enemyVisible) + Player.GetAutoAttackDamage(enemyVisible, true) * 2
                         > enemyVisible.Health)
                {
                    Drawing.DrawText(
                        Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                        Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                        Color.OrangeRed,
                        "Combo + 2 AA = OneShot");
                }
                else if (ComboDamage(enemyVisible) + Player.GetAutoAttackDamage(enemyVisible, true) * 5
                         > enemyVisible.Health)
                {
                    Drawing.DrawText(
                        Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                        Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                        Color.Orange,
                        "Ulti Combo = OneShot");
                }
                else
                    Drawing.DrawText(
                        Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                        Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                        Color.Green,
                        "No Kill");
            }


        }

        private static float ComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;
            var _igniteSlot = Player.GetSpellSlotFromName("SummonerDot");
            if (_igniteSlot != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(_igniteSlot) == SpellState.Ready) damage += ObjectManager.Player.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite);
            if (Item.HasItem(3077) && Item.CanUseItem(3077)) damage += Player.GetItemDamage(enemy, ItemId.Tiamat_Melee_Only);
            if (Item.HasItem(3074) && Item.CanUseItem(3074)) damage += Player.GetItemDamage(enemy, ItemId.Ravenous_Hydra_Melee_Only);
            if (Q.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.Q);
            if (W.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.W);
            if (E.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.E);
            damage += (damage - ObjectManager.Player.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite));
            return (float)damage;
        }

        private static void Combo()
        {
            var qtarget = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
            var wtarget = TargetSelector.GetTarget(W.Range, DamageType.Magical);
            var etarget = TargetSelector.GetTarget(E.Range, DamageType.Physical);
            var ePredCmb = E.GetPrediction(etarget).HitChance;
            var EoutRange = ComboMenu["Rengar.EoutRange"].Cast<CheckBox>().CurrentValue;
            var hydra = Player.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Ravenous_Hydra_Melee_Only);
            var tiamat = Player.InventoryItems.FirstOrDefault(a => a.Id == ItemId.Tiamat_Melee_Only);

            if (EoutRange && E.IsReady() && etarget.IsValidTarget() && E.IsInRange(etarget)
                && ePredCmb >= HitChance.High)
            {
                E.Cast(etarget);

                if (Player.Mana == 5 && Q.IsReady() && qtarget.IsValidTarget() && Q.IsInRange(qtarget))
                {
                    Q.Cast(qtarget);

                    if (hydra != null && hydra.CanUseItem() | tiamat != null && tiamat.CanUseItem())
                    {
                        tiamat.Cast();
                        hydra.Cast();

                        if (Player.Mana <= 4 && Q.IsReady() && qtarget.IsValidTarget() && Q.IsInRange(qtarget))
                        {
                            Q.Cast(qtarget);

                            if (Player.Mana <= 4 && W.IsReady() && wtarget.IsValidTarget() && W.IsInRange(wtarget))
                            {
                                W.Cast(wtarget);

                                if (Player.Mana <= 4 && E.IsReady() && etarget.IsValidTarget() && E.IsInRange(etarget)
                                    && ePredCmb >= HitChance.High)
                                {
                                    E.Cast(etarget);

                                }
                            }
                        }
                    }
                }
            }

            if (!EoutRange && W.IsReady() && wtarget.IsValidTarget() && W.IsInRange(wtarget))
            {
                W.Cast(wtarget);

                if (Player.Mana == 5 && Q.IsReady() && qtarget.IsValidTarget() && Q.IsInRange(qtarget))
                {
                    Q.Cast(qtarget);

                    if (hydra != null && hydra.CanUseItem() | tiamat != null && tiamat.CanUseItem())
                    {
                        tiamat.Cast();
                        hydra.Cast();

                        if (Player.Mana <= 4 && E.IsReady() && etarget.IsValidTarget() && E.IsInRange(etarget)
                            && ePredCmb >= HitChance.High)
                        {
                            E.Cast(etarget);

                            if (Player.Mana <= 4 && Q.IsReady() && qtarget.IsValidTarget() && Q.IsInRange(qtarget))
                            {
                                Q.Cast(qtarget);

                                if (Player.Mana <= 4 && W.IsReady() && wtarget.IsValidTarget() && W.IsInRange(wtarget))
                                {
                                    W.Cast(wtarget);

                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
