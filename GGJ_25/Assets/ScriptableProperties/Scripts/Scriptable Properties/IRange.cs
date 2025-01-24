using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    public interface IRange<T>
    {
        /// <returns>Minium value of property</returns>
        T GetMin();
        /// <returns>Maximum value of property</returns>
        T GetMax();
        /// <summary>
        /// Changes the min value
        /// </summary>
        void SetMin(T value);
        /// <summary>
        /// Changes the max value
        /// </summary>
        void SetMax(T value);
        /// <summary>
        /// Ensures that the max value is bigger than the min value and will flip the values if it is not. 
        /// </summary>
        void SwapMinMax();
        /// <summary>
        /// Ensures that value is within the min and max range. 
        /// </summary>
        void KeepInRange();
    }
}