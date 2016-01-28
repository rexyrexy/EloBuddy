﻿using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using System.Linq;

namespace RengarPro_Revamped
{
    class Standarts
    {
        public static Spell.Active Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange() + 100);
        public static Spell.Active W = new Spell.Active(SpellSlot.W, (uint)Player.Instance.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
        public static Spell.Skillshot E = new Spell.Skillshot(SpellSlot.E, (uint)Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.CastRange, SkillShotType.Linear, (int)Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.CastTime, (int)Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.MissileSpeed, (int)Player.Instance.Spellbook.GetSpell(SpellSlot.E).SData.LineWidth);
        public static Spell.Active R = new Spell.Active(SpellSlot.R, 2500);
        public static AIHeroClient Rengar
        {
            get
            {
                return Player.Instance;
            }
        }
        public static int Ferocity
        {
            get
            {
                return (int)Player.Instance.Mana;
            }
        } 
        public static bool RengarHasPassive
        {
            get
            {
                return Player.Instance.HasBuff("rengarpassivebuff");
            }
        }
        public static bool RengarHasUltimate
        {
            get
            {
                return Player.Instance.HasBuff("RengarR");
            }
        }
        public static bool RengarQ
        {
            get
            {
                return Rengar.Buffs.Any(x => x.Name.Contains("rengarq"));
            }
        }
    }
}