using System.Collections.Generic;

namespace ANT
{
    public static partial class Extensions
    {
        #if UNITY_EDITOR
        /// <summary>
        /// Searches the project for every asset of a specific type. 
        /// </summary>
        /// <typeparam name="T">the type you are looking to find</typeparam>
        /// <returns>All the assets in the project of that type</returns>
        public static List<T> FindAssetsByType<T>() where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = UnityEditor.AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                T asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
        #endif
    }
}