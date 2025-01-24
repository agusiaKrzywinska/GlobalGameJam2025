using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Converts a hexcode into a Unity Color. 
        /// </summary>
        /// <param name="color">The unity color object to wish to set to the new color.</param>
        /// <param name="hexCode">The hexcode for the color you would like.</param>
        /// <returns>A unity color with the new color containing that hexvalue.</returns>
        public static Color GetHexColor(this Color color, string hexCode)
        {
            ColorUtility.TryParseHtmlString(hexCode, out color);
            return color;
        } 
    }
}