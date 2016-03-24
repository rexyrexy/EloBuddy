using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace RyzePro
{
    class Spells
    {
        public static Spell.Skillshot Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1400, 50);
        public static Spell.Targeted W = new Spell.Targeted(SpellSlot.W,
            (uint)Starting.Ryze.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
        public static Spell.Targeted E = new Spell.Targeted(SpellSlot.E,
            (uint)Starting.Ryze.Spellbook.GetSpell(SpellSlot.W).SData.CastRange);
        public static Spell.Active R = new Spell.Active(SpellSlot.R);
        public static SpellSlot Ignite = Starting.Ryze.GetSpellSlotFromName("summonerdot");

    }
}
