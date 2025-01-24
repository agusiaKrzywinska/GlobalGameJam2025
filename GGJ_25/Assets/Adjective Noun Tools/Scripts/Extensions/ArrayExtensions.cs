using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Used in 2d arrays to confirm that there is an element that actually exists at that position. 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you are checking against</param>
        /// <param name="position">The position you want to confirm exists</param>
        /// <returns>If the 2d array element exists.</returns>
        public static bool Exists<T>(this T[,] array, Vector2Int position)
        {
            return position.x >= 0 && position.x < array.GetLength(0) && position.y >= 0 && position.y < array.GetLength(1);
        }

        /// <summary>
        /// Used in 2d arrays to get a specific element based on a position. 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you retrieving from</param>
        /// <param name="position">The position you want to get</param>
        /// <returns>The element it's it doesn't exist then it returns the default element.</returns>
        public static T GetElement<T>(this T[,] array, Vector2Int position)
        {
            if (array.Exists(position) == false)
                return default;

            return array[position.x, position.y];
        }

        /// <summary>
        /// Used in 2d arrays to change a specific element at a position if a position doesn't exist then it will 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you changing a value from</param>
        /// <param name="position">The position of the element you want to change</param>
        /// <param name="value">The new value you want to change it to.</param>
        public static void SetElement<T>(this T[,] array, Vector2Int position, T value)
        {
            if (array.Exists(position) == false)
                return;

            array[position.x, position.y] = value;

        }

        /// <summary>
        /// Used in arrays to confirm that there is an element that actually exists at that position. 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you are checking against</param>
        /// <param name="position">The position you want to confirm exists</param>
        /// <returns>If the array element exists.</returns>
        public static bool Exists<T>(this T[] array, int position)
        {
            return position >= 0 && position < array.Length;
        }

        /// <summary>
        /// Used in arrays to get a specific element based on a position. 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you retrieving from</param>
        /// <param name="position">The position you want to get</param>
        /// <returns>The element it's it doesn't exist then it returns the default element.</returns>
        public static T GetElement<T>(this T[] array, int position)
        {
            if (array.Exists(position) == false)
                return default;

            return array[position];
        }

        /// <summary>
        /// Used in arrays to change a specific element at a position if a position doesn't exist then it will 
        /// </summary>
        /// <typeparam name="T">The array type.</typeparam>
        /// <param name="array">The array that you changing a value from</param>
        /// <param name="position">The position of the element you want to change</param>
        /// <param name="value">The new value you want to change it to.</param>
        public static void SetElement<T>(this T[] array, int position, T value)
        {
            if (array.Exists(position) == false)
                return;

            array[position] = value;

        }

        /// <summary>
        /// Used for arrays to determine if it is empty which includes if it doesn't exist. 
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="array">the array you want to check</param>
        /// <returns>if the array is empty or is not instantiated.</returns>
        public static bool IsEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>
        /// Used to get a random element from a weighted array. 
        /// </summary>
        /// <typeparam name="T">the array type</typeparam>
        /// <param name="weightedObjects">the weighted objects that it can choose from.</param>
        /// <returns></returns>
        public static T GetItem<T>(this WeightedObject<T>[] weightedObjects)
        {
            int sum = weightedObjects.Sum(x => x.weightAmount);
            int elementID = Random.Range(1, sum + 1);

            int total = 0;
            for (int i = 0; i < weightedObjects.Length; i++)
            {
                total += weightedObjects[i].weightAmount;
                if (total >= elementID)
                {
                    return weightedObjects[i]._object;
                }
            }
            Debug.LogError($"Could not find element at {elementID} with a sum of {sum}");
            return default;
        }

        [System.Serializable]
        public class WeightedObject<T>
        {
            public int weightAmount;
            public T _object;
        }
    }
}