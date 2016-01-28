using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RyzePro
{
    class Spells
    {
        public static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q,
            (uint) Starting.Ryze.Spellbook.GetSpell(SpellSlot.Q).SData.CastRange, SkillShotType.Linear,
            (int) Starting.Ryze.Spellbook.GetSpell(SpellSlot.Q).SData.CastTime,
            (int) Starting.Ryze.Spellbook.GetSpell(SpellSlot.Q).SData.MissileSpeed);
        public static Spell.Targeted W = new Spell.Targeted(SpellSlot.W,
            (uint)Starting.Ryze.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
        public static Spell.Targeted E = new Spell.Targeted(SpellSlot.E,
            (uint)Starting.Ryze.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
        public static Spell.Active R = new Spell.Active(SpellSlot.R);
        public static SpellSlot Ignite = Starting.Ryze.GetSpellSlotFromName("summonerdot");

    }
}
