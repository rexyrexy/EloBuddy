using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Constants;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Utils;

namespace First_Class_Rengar
{
   public class Standarts
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Skillshot E;
        public static Spell.Active R;
        public static int LastSwitch;
        public static void SpellSs()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)(Me.GetAutoAttackRange(Me) + 100));
            W = new Spell.Active(SpellSlot.W, 500);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 70);
            R = new Spell.Active(SpellSlot.R, 2000);
        } 

        
        public static int Ferocity
        {
            get
            {
                return (int)Me.Mana;
            }
        }
        public static bool HasPassive
        {
            get
            {
                return Me.HasBuff("rengarpassivebuff");
            }
        }

        protected static AIHeroClient Me
        {
            get
            {
                return Player.Instance;
            }
        }

        protected static bool RengarR
        {
            get
            {
                return Me.Buffs.Any(x => x.Name.Contains("RengarR"));
            }
        }
    }
}
    
