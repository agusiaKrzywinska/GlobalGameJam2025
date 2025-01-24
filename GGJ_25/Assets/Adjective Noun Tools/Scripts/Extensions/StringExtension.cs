using System.Collections;
using System.Collections.Generic;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Will replace the a segment of a string with a different string. 
        /// </summary>
        /// <param name="s">the string you are modifying.</param>
        /// <param name="oldValue">The substring of the old value you are replacing.</param>
        /// <param name="newValue">The new substring that will be added in place of the old string.</param>
        /// <returns>The new string after replacing the first old value with the new value.</returns>
        public static string ReplaceFirstOccurrence(this string s, string oldValue, string newValue)
        {
            int i = s.IndexOf(oldValue);
            if (i < 0)
            {
                return oldValue;
            }
            return s.Remove(i, oldValue.Length).Insert(i, newValue);
        }

        /// <summary>
        /// Returns a list of all the indexes of a substring. 
        /// </summary>
        /// <param name="text">The string you are checking against.</param>
        /// <param name="str">The substring you are checking to see if it exists.</param>
        /// <returns>A list of all the indexes that contain that substring.</returns>
        public static List<int> AllIndexOf(this string text, string str)
        {
            List<int> allIndexOf = new List<int>();
            int index = text.IndexOf(str);
            while (index != -1)
            {
                allIndexOf.Add(index);
                index = text.IndexOf(str, index + 1);
            }
            return allIndexOf;
        }

        /// <summary>
        /// Will state if the string is empty. It will be empty if it's null or if has there is no actual value associated in that string. 
        /// </summary>
        /// <param name="s">The string you are checking if it is empty</param>
        /// <returns>If the string is empty or not.</returns>
        public static bool IsEmpty(this string s)
        {
            if (s == null)
                return true;

            var trimmed = s.Trim();
            if (trimmed.Length == 0 || trimmed == "")
                return true;

            return false;
        }
    }
}