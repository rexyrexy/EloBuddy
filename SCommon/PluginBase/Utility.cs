using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Utils;
using EloBuddy.SDK.ThirdParty;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Constants;

namespace SCommon.PluginBase
{
    internal static class Utility
    {
        #region Champion Priority Arrays
        public static string[] lowPriority =
            {
                "Alistar", "Amumu", "Bard", "Blitzcrank", "Braum", "Cho'Gath", "Dr. Mundo", "Garen", "Gnar",
                "Hecarim", "Janna", "Jarvan IV", "Leona", "Lulu", "Malphite", "Nami", "Nasus", "Nautilus", "Nunu",
                "Olaf", "Rammus", "Renekton", "Sejuani", "Shen", "Shyvana", "Singed", "Sion", "Skarner", "Sona",
                "Soraka", "Tahm", "Taric", "Thresh", "Volibear", "Warwick", "MonkeyKing", "Yorick", "Zac", "Zyra"
            };

        public static string[] mediumPriority =
            {
                "Aatrox", "Akali", "Darius", "Diana", "Ekko", "Elise", "Evelynn", "Fiddlesticks", "Fiora", "Fizz",
                "Galio", "Gangplank", "Gragas", "Heimerdinger", "Irelia", "Jax", "Jayce", "Kassadin", "Kayle", "Kha'Zix",
                "Lee Sin", "Lissandra", "Maokai", "Mordekaiser", "Morgana", "Nocturne", "Nidalee", "Pantheon", "Poppy",
                "RekSai", "Rengar", "Riven", "Rumble", "Ryze", "Shaco", "Swain", "Trundle", "Tryndamere", "Udyr",
                "Urgot", "Vladimir", "Vi", "XinZhao", "Yasuo", "Zilean"
            };

        public static string[] highPriority =
            {
                "Ahri", "Anivia", "Annie", "Ashe", "Azir", "Brand", "Caitlyn", "Cassiopeia", "Corki", "Draven",
                "Ezreal", "Graves", "Jinx", "Kalista", "Karma", "Karthus", "Katarina", "Kennen", "KogMaw", "Leblanc",
                "Lucian", "Lux", "Malzahar", "MasterYi", "MissFortune", "Orianna", "Quinn", "Sivir", "Syndra", "Talon",
                "Teemo", "Tristana", "TwistedFate", "Twitch", "Varus", "Vayne", "Veigar", "VelKoz", "Viktor", "Xerath",
                "Zed", "Ziggs"
            };
        #endregion

        public static string[] HitchanceNameArray = { "Low", "Medium", "High", "Very High", "Only Immobile" };
        public static HitChance[] HitchanceArray = { HitChance.Low, HitChance.Medium, HitChance.High, HitChance.Dashing, HitChance.Immobile };

        private static string[] JungleMinions;

        static Utility()
        {
            if (Game.MapId == GameMapId.TwistedTreeline)
            {
                JungleMinions = new string[] { "TT_Spiderboss", "TT_NWraith", "TT_NGolem", "TT_NWolf" };
            }
            else
            {
                JungleMinions = new string[]
                    {
                        "SRU_Blue", "SRU_Gromp", "SRU_Murkwolf", "SRU_Razorbeak", "SRU_Red", "SRU_Krug", "SRU_Dragon",
                        "SRU_Baron", "Sru_Crab"
                    };
            }
        }

        public static int GetPriority(string championName)
        {
            if (lowPriority.Contains(championName))
                return 1;

            if (mediumPriority.Contains(championName))
                return 2;

            if (highPriority.Contains(championName))
                return 3;

            return 2;
        }

        public static bool IsJungleMinion(this Obj_AI_Base unit)
        {
            return JungleMinions.Contains(unit.Name);
        }

        public static bool IsImmobilizeBuff(BuffType type)
        {
            return type == BuffType.Snare || type == BuffType.Stun || type == BuffType.Charm || type == BuffType.Knockup || type == BuffType.Suppression;
        }

        public static bool IsImmobileTarget(this AIHeroClient target)
        {
            return target.Buffs.Count(p => IsImmobilizeBuff(p.Type)) > 0 || target.Spellbook.IsChanneling;
        }

        public static bool IsActive(this Spell.SpellBase s)
        {
            return ObjectManager.Player.Spellbook.GetSpell(s.Slot).ToggleState == 2;
        }
    }
}
