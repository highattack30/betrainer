﻿//////////////////////////////////////////////
//   BE Trainer.NET for Grand Theft Auto V
//             by BE.Tenner
//      Copyright (c) BE Group 2015
//               Thanks to
//    ScriptHookV & ScriptHookVDotNet
//  Native Trainer & Enhanced Native Trainer
//////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using GTA;
using GTA.Native;
using System.Windows.Forms;

namespace GTAVBETrainerDotNet
{
    /// <summary>
    /// Utilities
    /// </summary>
    public static class Utils
    {
        /// <summary>
        /// Shows a notification above in-game mini-map
        /// </summary>
        /// <param name="message">Message to show</param>
        public static void ShowNotificationAboveMap(MString message)
        {
            Function.Call(Hash._SET_NOTIFICATION_TEXT_ENTRY, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, message[Trainer.LanguageCode]);
            Function.Call(Hash._DRAW_NOTIFICATION, 0, 1);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="x">X position in pixel</param>
        /// <param name="y">Y position in pixel</param>
        /// <param name="width">Width in pixel</param>
        /// <param name="height">Height in pixel</param>
        /// <param name="color">Color</param>
        /// <param name="screenWidth">Screen width in pixel</param>
        /// <param name="screenHeight">Screen height in pixel</param>
        public static void DrawRect(int x, int y, int width, int height, Color color, int screenWidth = GlobalConst.DEFAULT_SCREEN_WIDTH, int screenHeight = GlobalConst.DEFAULT_SCREEN_HEIGHT)
        {
            float xp = (float)x / screenWidth;
            float yp = (float)y / screenHeight;
            float wp = (float)width / screenWidth;
            float hp = (float)height / screenHeight;
            Function.Call(Hash.DRAW_RECT, xp + wp / 2f, yp + hp / 2f, wp, hp, color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Draw text
        /// </summary>
        /// <param name="text">Text to drawn</param>
        /// <param name="x">X position in pixel</param>
        /// <param name="y">Y position in pixel</param>
        /// <param name="align">Horizontal alignment</param>
        /// <param name="color">Color</param>
        /// <param name="xScale">X Scale</param>
        /// <param name="yScale">Y Scale</param>
        /// <param name="font">Font</param>
        /// <param name="shadowOffset">Offset of shadow</param>
        /// <param name="shadowColor">Color of shadow</param>
        /// <param name="screenHeight">Height of screen in pixel</param>
        /// <param name="screenWidth">Width of screen in pixel</param>
        public static void DrawText(MString text, int x, int y, GlobalConst.HAlign align, Color color, float xScale = 0.35f, float yScale = 0.35f, GTA.Font font = GTA.Font.ChaletLondon, Point shadowOffset = new Point(), Color shadowColor = new Color(), int screenWidth = GlobalConst.DEFAULT_SCREEN_WIDTH, int screenHeight = GlobalConst.DEFAULT_SCREEN_HEIGHT)
        {
            if (shadowOffset.X != 0 || shadowOffset.Y != 0)
            {
                DrawText(text, x + shadowOffset.X, y + shadowOffset.Y, align, shadowColor, xScale, yScale, font, new Point(), Color.Black, screenWidth, screenHeight);
            }
            Function.Call(Hash.SET_TEXT_FONT, (int)font);
            Function.Call(Hash.SET_TEXT_SCALE, xScale, yScale);
            Function.Call(Hash.SET_TEXT_COLOUR, color.R, color.G, color.B, color.A);
            switch (align)
            {
                case GlobalConst.HAlign.Left:
                    Function.Call(Hash.SET_TEXT_CENTRE, 0);
                    break;

                case GlobalConst.HAlign.Center:
                    Function.Call(Hash.SET_TEXT_CENTRE, 1);
                    break;

                case GlobalConst.HAlign.Right:
                    Function.Call(Hash.SET_TEXT_RIGHT_JUSTIFY, 1);
                    break;
            }
            Function.Call(Hash._SET_TEXT_ENTRY, "STRING");
            Function.Call(Hash._ADD_TEXT_COMPONENT_STRING, ML(text));
            Function.Call(Hash._DRAW_TEXT, (float)x / screenWidth, (float)y / screenHeight);
        }

        /// <summary>
        /// Play a sound
        /// </summary>
        /// <param name="sound">Sound name</param>
        /// <param name="soundSet">Sound set name</param>
        public static void PlaySound(string sound, string soundSet)
        {
            Function.Call(Hash.PLAY_SOUND_FRONTEND, -1, sound, soundSet, 0);
        }

        /// <summary>
        /// Shows the in-game keyboard screen and gets the inputted text.
        /// </summary>
        /// <param name="titleId">Title ID (not the title itself!)</param>
        /// <param name="prePopulatedText">Text shows in the input area after keyboard open</param>
        /// <param name="maxInputLength">Max input length</param>
        /// <returns>User inputted text or null if nothing inputted.</returns>
        public static string ShowInGameKeyboard(string titleId = null, string prePopulatedText = null, int maxInputLength = 64)
        {
            Script.Wait(100);

            Function.Call(Hash.DISPLAY_ONSCREEN_KEYBOARD, true, (titleId == null ? "HUD_TITLE" : titleId), "", (prePopulatedText == null ? "" : prePopulatedText), "", "", "", maxInputLength + 1);

            while (Function.Call<int>(Hash.UPDATE_ONSCREEN_KEYBOARD) == 0)
            {
                Script.Wait(0);
            }

            return Function.Call<string>(Hash.GET_ONSCREEN_KEYBOARD_RESULT);
        }

        /// <summary>
        /// Parses an int value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static int ParseInt(string value)
        {
            int v = 0;
            if (int.TryParse(value, out v)) return v;
            return 0;
        }

        /// <summary>
        /// Parses an float value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static float ParseFloat(string value)
        {
            float v = 0f;
            if (float.TryParse(value, out v)) return v;
            return 0f;
        }

        /// <summary>
        /// Parses a boolean value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static bool ParseBool(string value)
        {
            int v = ParseInt(value);
            return (v == 1);
        }

        /// <summary>
        /// Parses a boolean string value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static bool ParseBoolStr(string value)
        {
            bool v = false;
            if (bool.TryParse(value, out v)) return v;
            return false;
        }

        /// <summary>
        /// Parses a boolean string value
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static bool ParseBoolStr(string value, bool defaultValue)
        {
            bool v = false;
            if (bool.TryParse(value, out v)) return v;
            return defaultValue;
        }

        /// <summary>
        /// Parses a nullable boolean value
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static bool? ParseNullableBool(string value)
        {
            int v = ParseInt(value);
            if (v == 0) return false;
            if (v == 1) return true;
            return null;
        }

        /// <summary>
        /// Parses a key
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public static Keys ParseKey(string value)
        {
            Keys key = Keys.None;
            if (Enum.TryParse<Keys>(value, out key)) return key;
            return Keys.None;
        }

        /// <summary>
        /// Generates a multi-string with a default string and a Chinese Traditional string
        /// </summary>
        /// <param name="defaultString">Default string</param>
        /// <param name="chineseTraditional">Chinese triditional string</param>
        /// <returns></returns>
        public static MString CTML(string defaultString, string chineseTraditional)
        {
            return new MString(defaultString, new KeyValuePair<int, string>(Language.CODE_CHINESE_TRADITIONAL, chineseTraditional));
        }

        /// <summary>
        /// Gets the string with current language code.
        /// </summary>
        /// <param name="str">Multi-string</param>
        /// <returns></returns>
        public static string ML(MString str)
        {
            return str[Trainer.LanguageCode];
        }

        public static string FormatML(MString format, params object[] args)
        {
            return string.Format(ML(format), args);
        }

        public static string FormatML(MString format, MString arg)
        {
            return string.Format(ML(format), ML(arg));
        }

    }
}
