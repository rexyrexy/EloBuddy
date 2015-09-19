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

namespace Damage_Calculator
{
    internal class Program
    {
        public static Menu DamageCalculateMenu;
        public static AIHeroClient Player
        {
            get
            {
                return ObjectManager.Player;
            }
        }

        public static Spell.SpellBase Q;

        public static Spell.SpellBase W;

        public static Spell.SpellBase E;

        public static Spell.SpellBase R;
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Hacks.AntiAFK = true;
            Bootstrap.Init(null);
            DamageCalculateMenu = MainMenu.AddMenu("Damage Calculator", "DamageCalculatorMenu");
            DamageCalculateMenu.AddGroupLabel("Damage Calculator");
            DamageCalculateMenu.Add("DamageCalculator.Enable", new CheckBox("Enable Draw"));
            DamageCalculateMenu.AddSeparator();
            DamageCalculateMenu.AddLabel("Coded by Rexy");
            DamageCalculateMenu.AddLabel("www.elobuddy.net");
            Chat.Print("Damage Calculator Loaded.");
            Chat.Print("Coded by Rexy");

            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static float ComboDamage(Obj_AI_Base enemy)
        {
            var damage = 0d;
            var igniteSlot = Player.GetSpellSlotFromName("SummonerDot");
            if (igniteSlot != SpellSlot.Unknown && Player.Spellbook.CanUseSpell(igniteSlot) == SpellState.Ready) damage += ObjectManager.Player.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite);
            if (Item.HasItem(3077) && Item.CanUseItem(3077)) damage += Player.GetItemDamage(enemy, ItemId.Tiamat_Melee_Only);
            if (Item.HasItem(3074) && Item.CanUseItem(3074)) damage += Player.GetItemDamage(enemy, ItemId.Ravenous_Hydra_Melee_Only);
            if (Item.HasItem(3144) && Item.CanUseItem(3144)) damage += Player.GetItemDamage(enemy, ItemId.Bilgewater_Cutlass);
            if (Item.HasItem(3153) && Item.CanUseItem(3153)) damage += Player.GetItemDamage(enemy, ItemId.Blade_of_the_Ruined_King);
            if (Item.HasItem(3146) && Item.CanUseItem(3146)) damage += Player.GetItemDamage(enemy, ItemId.Hextech_Gunblade);

            if (Q.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.Q);
            if (W.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.W);
            if (E.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.E);
            if (R.IsReady()) damage += Player.GetSpellDamage(enemy, SpellSlot.R);
            damage += (damage - ObjectManager.Player.GetSummonerSpellDamage(enemy, DamageLibrary.SummonerSpells.Ignite));
            return (float)damage;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var activateDrawings = DamageCalculateMenu["DamageCalculator.Enable"].Cast<CheckBox>().CurrentValue;

            if (activateDrawings)
            {
                foreach (var enemyVisible in
                    ObjectManager.Get<Obj_AI_Base>()
                        .Where(enemyVisible => enemyVisible.IsValidTarget() && enemyVisible.IsEnemy))
                {
                    if (ComboDamage(enemyVisible) > enemyVisible.Health)
                    {
                        Drawing.DrawText(
                            Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                            Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                            Color.Red,
                            "Combo = Kill");
                    }
                    else if (ComboDamage(enemyVisible) + Player.GetAutoAttackDamage(enemyVisible, true) * 2
                             > enemyVisible.Health)
                    {
                        Drawing.DrawText(
                            Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                            Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                            Color.OrangeRed,
                            "Combo + 2 AA = Kill");
                    }
                    else
                        Drawing.DrawText(
                            Drawing.WorldToScreen(enemyVisible.Position)[0] + 50,
                            Drawing.WorldToScreen(enemyVisible.Position)[1] - 40,
                            Color.Green,
                            "No Kill");
                }
            }
        }
    }
}
