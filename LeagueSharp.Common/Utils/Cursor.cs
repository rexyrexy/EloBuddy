﻿using EloBuddy;
using SharpDX;

namespace LeagueSharp.Common
{
    /// <summary>
    ///     The cursor tracker class.
    /// </summary>
    internal class Cursor
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a static instance of the <see cref="ColorPicker" /> class.
        /// </summary>
        static Cursor()
        {
            Game.OnWndProc += OnWndProc;
        }

        #endregion

        #region Static Fields

        /// <summary>
        ///     The cursor X-axis pos.
        /// </summary>
        private static int posX;

        /// <summary>
        ///     The cursor Y-axis pos.
        /// </summary>
        private static int posY;

        #endregion

        #region Methods

        /// <summary>
        ///     Gets the cursor current position on screen.
        /// </summary>
        /// <returns>
        ///     The cursor current position.
        /// </returns>
        internal static Vector2 GetCursorPos()
        {
            return new Vector2(posX, posY);
        }

        /// <summary>
        ///     The windows process messages event.
        /// </summary>
        /// <param name="args">
        ///     The arguments.
        /// </param>
        private static void OnWndProc(WndEventArgs args)
        {
            if (args.Msg == (uint)(WindowsMessages.WM_MOUSEMOVE))
            {
                unchecked
                {
                    posX = (short) args.LParam;
                    posY = (short) ((long) args.LParam >> 16);
                }
            }
        }

        #endregion
    }
}