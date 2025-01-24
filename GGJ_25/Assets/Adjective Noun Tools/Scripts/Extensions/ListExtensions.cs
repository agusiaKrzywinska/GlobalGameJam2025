using System.Collections.Generic;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Used for lists to determine if it is empty which includes if it doesn't exist. 
        /// </summary>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="list">the array you want to check</param>
        /// <returns>if the list is empty or is not instantiated.</returns>
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Used for lists to retrieve an element at a specific position even if that element doesn't exist. 
        /// </summary>
        /// <typeparam name="T">the list type</typeparam>
        /// <param name="list">the list you want to retrieve from</param>
        /// <param name="pos">the position of the element you are attempting to retrieve.</param>
        /// <returns></returns>
        public static T GetElement<T>(this List<T> list, int pos)
        {
            if(pos < 0 || pos >= list.Count)
                return default;

            return list[pos];
        }


        /// <summary>
        /// Will swap 2 elements in a list. 
        /// </summary>
        /// <typeparam name="T">the list type</typeparam>
        /// <param name="list">the list that contains the elements you want to swap</param>
        /// <param name="index1">the first element index you want to swap</param>
        /// <param name="index2">the second element index you want to swap</param>
        public static void SwapElements<T>(this List<T> list, int index1, int index2)
        {
            var temp = list.GetElement(index1);
            list[index1] = list.GetElement(index2);
            list[index2] = temp;
        }
    }
}