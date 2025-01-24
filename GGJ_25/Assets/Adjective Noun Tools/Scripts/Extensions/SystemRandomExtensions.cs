using System.Collections;
using System.Collections.Generic;

namespace ANT
{
    public static partial class Extensions
    {
        /// <summary>
        /// Returns a random float within minInclusive, maxInclusive. 
        /// </summary>
        /// <param name="rng">The random object you are using to generate it.</param>
        /// <param name="min">The minimum value inclusive.</param>
        /// <param name="max">The maximum value inclusive</param>
        /// <returns>A float between min and max.</returns>
        public static float Range(this System.Random rng, float min, float max)
        {
            return (float)(rng.NextDouble() * (max - min) + min);
        }

        /// <summary>
        /// Returns a random float within 0, maxInclusive. 
        /// </summary>
        /// <param name="rng">The random object you are using to generate it.</param>
        /// <param name="max">The maximum value inclusive</param>
        /// <returns>A float between 0 and max.</returns>
        public static float Range(this System.Random rng, float max)
        {
            return rng.Range(0, max);
        }
    }
}