using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace RengarPro_Revamped.Helper
{
    class Misc
    {
        public static int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };
        public static int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };
        public static SpellSlot Smite;
        public static void Init()
        {
            Game.OnTick += Game_OnTick;
        }
        public static void Game_OnTick(EventArgs args)
        {
            AutoHp();
            SmiteCombo();
            SkinHack();
            AutoYoumuu();
            BetaQ();
        }
        public static void BetaQ()
        {
            if (TargetSelector.SelectedTarget.IsValidTarget(875) && MenuChecker.ComboModeSelected == 1 && Standarts.RengarHasUltimate && Standarts.Q.IsReady())
            {
                Standarts.Q.Cast();
                Orbwalking.ResetAutoAttackTimer();
            }
        }

        public static void AutoYoumuu()
        {
            if (MenuChecker.AutoYoumuuActive && Standarts.RengarHasUltimate && Item.CanUseItem(ItemId.Youmuus_Ghostblade))
            {
                Core.DelayAction(() => Item.UseItem(ItemId.Youmuus_Ghostblade), 600);
            }
        }

        public static void SkinHack()
        {
            if (!MenuChecker.SkinHackActive)
            {
                Standarts.Rengar.SetSkinId(0);
                return;
            }

            switch (MenuChecker.SkinHackValue)
            {
                case 1: { Standarts.Rengar.SetSkinId(1); break; }
                case 2: { Standarts.Rengar.SetSkinId(2); break; }
                case 3: { Standarts.Rengar.SetSkinId(3); break; }
            }
        }

        public static void SmiteCombo()
        {
            if (BlueSmite.Any(id => Item.HasItem(id)))
            {
                Smite = Standarts.Rengar.GetSpellSlotFromName("s5_summonersmiteplayerganker");
                return;
            }

            if (RedSmite.Any(id => Item.HasItem(id)))
            {
                Smite = Standarts.Rengar.GetSpellSlotFromName("s5_summonersmiteduel");
                return;
            }

            Smite = Standarts.Rengar.GetSpellSlotFromName("summonersmite");
        }

        public static void AutoHp()
        {
            if (!MenuChecker.AutoHpActive)
            {
                return;
            }
            if (Standarts.Rengar.HealthPercent <= MenuChecker.AutoHpValue && Standarts.Ferocity == 5 && Standarts.W.IsReady())
            {
                Standarts.W.Cast();
            }
        }
    }
}
