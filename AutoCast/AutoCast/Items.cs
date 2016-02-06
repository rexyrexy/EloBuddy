using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace AutoCast
{
    class Items
    {
        public static AIHeroClient EnemyTarget;
        public static int[] BlueSmite = { 3706, 1400, 1401, 1402, 1403 };
        public static int[] RedSmite = { 3715, 1415, 1414, 1413, 1412 };
        public static SpellSlot Smite;
        public static void GetSmite()
        {
            if (BlueSmite.Any(id => Item.HasItem(id)))
            {
                Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteplayerganker");
                return;
            }

            if (RedSmite.Any(id => Item.HasItem(id)))
            {
                Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteduel");
                return;
            }

            Smite = Player.Instance.GetSpellSlotFromName("summonersmite");
        }
        public static void CastHydra()
        {
            if (!Player.Instance.IsVisible)
            {
                return;
            }
            Targetting();
            if (Item.HasItem(ItemId.Ravenous_Hydra_Melee_Only) && Item.CanUseItem(ItemId.Ravenous_Hydra_Melee_Only)
                && MenuCheck.CastHydra)
            {
                if (EnemyTarget.IsValidTarget(400))
                { 
                Item.UseItem(ItemId.Ravenous_Hydra_Melee_Only);
                }
            }
        }

        public static void CastTiamat()
        {
            if (!Player.Instance.IsVisible)
            {
                return;
            }
            Targetting();
            if (Item.HasItem(ItemId.Tiamat_Melee_Only) && Item.CanUseItem(ItemId.Tiamat_Melee_Only)
                && MenuCheck.CastTiamat)
            {
                if (EnemyTarget.IsValidTarget(400))
                {
                    Item.UseItem(ItemId.Tiamat_Melee_Only);
                }
            }
        }
        
        public static void Targetting()
        {
            if (!TargetSelector.SelectedTarget.IsValidTarget(1200))
            {
                EnemyTarget = TargetSelector.GetTarget(1200, DamageType.Physical);
            }
            else if (TargetSelector.SelectedTarget.IsValidTarget(1200))
            {
                EnemyTarget = TargetSelector.SelectedTarget;
            }

        }

        public static void CastSmite()
        {
            if (!Player.Instance.IsVisible)
            {
                return;
            }
            Targetting();
            if (MenuCheck.CastSmite && !Player.Instance.HasBuff("RengarR") && Smite != SpellSlot.Unknown
                        && Player.Instance.Spellbook.CanUseSpell(Smite) == SpellState.Ready && EnemyTarget.IsValidTarget(500))
            {
                Player.Instance.Spellbook.CastSpell(Smite, EnemyTarget);
            }
        }
        
    }
}
