/*
 Copyright 2015 - 2015 SPrediction
 ConfigMenu.cs is part of SPrediction
 
 SPrediction is free software: you can redistribute it and/or modify
 it under the terms of the GNU General Public License as published by
 the Free Software Foundation, either version 3 of the License, or
 (at your option) any later version.
 
 SPrediction is distributed in the hope that it will be useful,
 but WITHOUT ANY WARRANTY; without even the implied warranty of
 MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 GNU General Public License for more details.
 
 You should have received a copy of the GNU General Public License
 along with SPrediction. If not, see <http://www.gnu.org/licenses/>.
*/

using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using LeagueSharp;
using LeagueSharp.Common;
using KeyBind = LeagueSharp.Common.KeyBind;
using Menu = LeagueSharp.Common.Menu;
using Slider = LeagueSharp.Common.Slider;

namespace SPrediction
{
    /// <summary>
    /// SPrediction Config Menu class
    /// </summary>
    public static class ConfigMenu
    {
        #region Private Properties

        public static EloBuddy.SDK.Menu.Menu s_Menu;

        #endregion

        #region Initalizer Method
        /// <summary>
        /// Creates the sprediciton menu
        /// </summary>
        public static void Initialize(string prefMenuName = "SPRED")
        {
            s_Menu = MainMenu.AddMenu("SPrediction", prefMenuName);
            s_Menu.AddLabel("0 => SPrediction || 1 => Common Prediction");
            s_Menu.Add("PREDICTONLIST", new EloBuddy.SDK.Menu.Values.Slider("Prediction Method", 0, 0, 1));
            s_Menu.Add("SPREDWINDUP", new CheckBox("Check for target AA Windup", false));
            s_Menu.Add("SPREDMAXRANGEIGNORE",
                new EloBuddy.SDK.Menu.Values.Slider("Max Range Dodge Ignore (%)", 50, 0, 100));
            s_Menu.Add("SPREDREACTIONDELAY", new EloBuddy.SDK.Menu.Values.Slider("Ignore Rection Delay", 0, 0, 200));
            s_Menu.Add("SPREDDELAY", new EloBuddy.SDK.Menu.Values.Slider("Spell Delay", 0, 0, 200));
            s_Menu.Add("SPREDHC",
                new EloBuddy.SDK.Menu.Values.KeyBind("Count HitChance", false,
                    EloBuddy.SDK.Menu.Values.KeyBind.BindTypes.HoldActive, 32));
            s_Menu.Add("SPREDDRAWINGX",
                new EloBuddy.SDK.Menu.Values.Slider("Drawing Pos X", Drawing.Width - 200, 0, Drawing.Width));

            s_Menu.Add("SPREDDRAWINGY",
                new EloBuddy.SDK.Menu.Values.Slider("Drawing Pos Y", 0, 0, Drawing.Height));
            s_Menu.Add("SPREDDRAWINGS", new CheckBox("Enable Drawings", false));
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets selected prediction for spell extensions
        /// </summary>
        public static int SelectedPrediction
        {
            get { return s_Menu["PREDICTONLIST"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue; }
        }

        /// <summary>
        /// Gets or sets Check AA WindUp value
        /// </summary>
        public static bool CheckAAWindUp
        {
            get { return s_Menu["SPREDWINDUP"].Cast<CheckBox>().CurrentValue; }
            set { s_Menu["SPREDWINDUP"].Cast<CheckBox>().CurrentValue = false; }
        }

        /// <summary>
        /// Gets or sets max range ignore value
        /// </summary>
        public static int MaxRangeIgnore
           {
            get
            {
                return s_Menu["SPREDMAXRANGEIGNORE"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue;
            }
           }

        public static int MaxRangeValue;
        public static int IgnoreReactionDelayValue;
        public static int SpellDelayValue;
        public static int HitChanceDrawingXValue;
        public static int HitChanceDrawingYValue;
        public static bool EnableDrawingsValue;

        public static EloBuddy.SDK.Menu.Menu MaxRangeIgnoreSet
        {
            set
            {
                s_Menu["SPREDMAXRANGEIGNORE"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue = MaxRangeValue;
                ;
            }
        }


        /// <summary>
        /// Gets or sets ignore reaction delay value
        /// </summary>
        public static int IgnoreReactionDelay
        {
            get { return s_Menu["SPREDREACTIONDELAY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue; }
            set { s_Menu["SPREDREACTIONDELAY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue = IgnoreReactionDelayValue; }
        }

        /// <summary>
        /// Gets or sets spell delay value
        /// </summary>
        public static int SpellDelay
        {
            get { return s_Menu["SPREDDELAY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue; }
            set { s_Menu["SPREDDELAY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue = SpellDelayValue; }
        }

        /// <summary>
        /// Gets count hitchance key is enabled
        /// </summary>
        public static bool CountHitChance
        {
            get { return s_Menu["SPREDHC"].Cast<EloBuddy.SDK.Menu.Values.KeyBind>().CurrentValue; }
        }

        /// <summary>
        /// Gets or sets drawing x pos for hitchance drawings
        /// </summary>
        public static int HitChanceDrawingX
        {
            get { return s_Menu["SPREDDRAWINGX"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue; }
            set { s_Menu["SPREDDRAWINGX"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue = HitChanceDrawingXValue; }
        }

        /// <summary>
        /// Gets or sets drawing y pos for hitchance drawings
        /// </summary>
        public static int HitChanceDrawingY
        {
            get { return s_Menu["SPREDDRAWINGY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue; }
            set { s_Menu["SPREDDRAWINGY"].Cast<EloBuddy.SDK.Menu.Values.Slider>().CurrentValue = HitChanceDrawingXValue; }
        }

        /// <summary>
        /// Gets or sets drawings are enabled
        /// </summary>
        public static bool EnableDrawings
        {
            get { return s_Menu["SPREDDRAWINGS"].Cast<CheckBox>().CurrentValue; }
            set { s_Menu["SPREDDRAWINGS"].Cast<CheckBox>().CurrentValue = EnableDrawingsValue; }
        }

        #endregion
    }
}
