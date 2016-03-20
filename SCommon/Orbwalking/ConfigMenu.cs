using System;
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
using Color = System.Drawing.Color;

namespace SCommon.Orbwalking
{
    public class ConfigMenu
    {
        private Menu m_Menu;
        private Orbwalker m_orbInstance;

        public ConfigMenu(Orbwalker instance, Menu menuToAttach)
        {
            m_Menu = MainMenu.AddMenu("Orbwalking", "Orbwalking.Root");
            Menu misc = m_Menu.AddSubMenu("Misc", "Orbwalking.Misc");
            misc.Add("Orbwalking.Misc.blAttackStructures", new CheckBox("Attack Structures"));
            misc.Add("Orbwalking.Misc.blFocusNormalWhileTurret", new CheckBox("Focus Last hit minion that not targetted from turret while under turret"));
           misc.AddLabel("if this option enabled, orbwalker first \ntry to last hit minions which \nthey are not attacked from turret and targetted by ally minions");
            misc.Add("Orbwalking.Misc.blSupportMode", new CheckBox("Support Mode", false));
            misc.Add("Orbwalking.Misc.blDontMoveMouseOver", new CheckBox("Mouse over hero to stop move", false));
            misc.Add("Orbwalking.Misc.blMagnetMelee", new CheckBox("Magnet Target (Only Melee)", false)).OnValueChange +=
             (s, ar) =>
                {
                    var it = misc["Orbwalking.Misc.iStickRange"].Cast<Slider>().CurrentValue;
                    if (it != null)
                        it.Equals(ar.NewValue);
                };
            misc.Add("Orbwalking.Misc.iStickRange", new Slider("Stick Range", 300, 0, 500));
            misc.Add("Orbwalking.Misc.blDontMoveInRange", new CheckBox("Dont move if enemy in AA range", false));
            misc.Add("Orbwalking.Misc.blLegitMode", new CheckBox("Legit Mode", false)).OnValueChange +=
                (s, ar) =>
                {
                    var it = misc["Orbwalking.Misc.iLegitPercent"].Cast<Slider>().CurrentValue;
                    if (it != null)
                        it.Equals(ar.NewValue);
                };
            misc.Add("Orbwalking.Misc.iLegitPercent", new Slider("Make Me Legit %", 20, 0, 100));
        }
        
        /// <summary>
        /// Gets combo key is pressed
        /// </summary>
        public bool Combo
        {
            get { return EloBuddy.SDK.Orbwalker.ActiveModesFlags.HasFlag(EloBuddy.SDK.Orbwalker.ActiveModes.Combo); }
        }

        /// <summary>
        /// Gets harass key is pressed
        /// </summary>
        public bool Harass
        {
            get { return EloBuddy.SDK.Orbwalker.ActiveModesFlags.HasFlag(EloBuddy.SDK.Orbwalker.ActiveModes.Harass); }
        }

        /// <summary>
        /// Gets lane clear key is pressed
        /// </summary>
        public bool LaneClear
        {
            get { return EloBuddy.SDK.Orbwalker.ActiveModesFlags.HasFlag(EloBuddy.SDK.Orbwalker.ActiveModes.LaneClear); }
        }

        /// <summary>
        /// Gets last hit key is pressed
        /// </summary>
        public bool LastHit
        {
            get { return EloBuddy.SDK.Orbwalker.ActiveModesFlags.HasFlag(EloBuddy.SDK.Orbwalker.ActiveModes.LastHit); }
        }

        /// <summary>
        /// Gets or sets extra windup time value
        /// </summary>
        public int ExtraWindup
        {
            get
            {
                return EloBuddy.SDK.Orbwalker.HoldRadius;
            }

        }

        /// <summary>
        /// Gets or sets movement delay value
        /// </summary>
        public int MovementDelay
        {
            get
            {
                return EloBuddy.SDK.Orbwalker.ExtraFarmDelay;
            }

        }

        /// <summary>
        /// Gets or sets hold area radius value
        /// </summary>
        public int HoldAreaRadius
        {
            get
            {
                return EloBuddy.SDK.Orbwalker.HoldRadius;
            }
        }

        /// <summary>
        /// Gets or sets attack structures value
        /// </summary>
        public bool AttackStructures
        {
            get
            {
                return m_Menu["Orbwalking.Misc.blAttackStructures"].Cast<CheckBox>().CurrentValue; ;
            }
             }

        /// <summary>
        /// Gets or sets focus normal while turret value
        /// </summary>
        public bool FocusNormalWhileTurret
        {
            get { return m_Menu["Orbwalking.Misc.blFocusNormalWhileTurret"].Cast<CheckBox>().CurrentValue; }
        }

        /// <summary>
        /// Gets or sets support mode value
        /// </summary>
        public bool SupportMode
        {
            get { return m_Menu["Orbwalking.Misc.blSupportMode"].Cast<CheckBox>().CurrentValue; }
        }
        
        /// <summary>
        /// Gets or sets Dont attack champions while laneclear mode value
        /// </summary>
        public bool DontAttackChampWhileLaneClear
        {
            get { return !EloBuddy.SDK.Orbwalker.LaneClearAttackChamps; }
            
        }

        /// <summary>
        /// Gets or sets Dont move over value
        /// </summary>
        public bool DontMoveMouseOver
        {
            get { return m_Menu["Orbwalking.Misc.blDontMoveMouseOver"].Cast<CheckBox>().CurrentValue; }
        }

        /// <summary>
        /// Gets or set magnet melee value
        /// </summary>
        public bool MagnetMelee
        {
            get { return m_Menu["Orbwalking.Misc.blMagnetMelee"].Cast<CheckBox>().CurrentValue; }
        }

        /// <summary>
        /// Gets or sets stick range value
        /// </summary>
        public int StickRange
        {
            get { return m_Menu["Orbwalking.Misc.iStickRange"].Cast<Slider>().CurrentValue; }
        }

        /// <summary>
        /// Gets or sets dont move in aa range value
        /// </summary>
        public bool DontMoveInRange
        {
            get { return m_Menu["Orbwalking.Misc.blDontMoveInRange"].Cast<CheckBox>().CurrentValue; }
        }

        /// <summary>
        /// Gets or set legit mode value
        /// </summary>
        public bool LegitMode
        {
            get { return m_Menu["Orbwalking.Misc.blLegitMode"].Cast<CheckBox>().CurrentValue && ObjectManager.Player.GetAttackSpeed() > 1.40f && m_orbInstance.ActiveMode == Orbwalker.Mode.Combo; }
        }

        /// <summary>
        /// Gets or set legit percent value
        /// </summary>
        public int LegitPercent
        {
            get { return m_Menu["Orbwalking.Misc.iLegitPercent"].Cast<Slider>().CurrentValue; }
        }
    }
}
