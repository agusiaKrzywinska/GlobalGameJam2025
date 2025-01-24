using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [DefaultExecutionOrder(-2)]
    public class ScriptableObjectRuntimeManager : MonoBehaviour
    {
        public List<ScriptableProperty> scriptables;
        protected static bool shouldUpdate = true;

        protected virtual void OnEnable()
        {
#if UNITY_EDITOR
            FindAllScriptableObjectsRuntime();
#endif 
            if (!shouldUpdate) return;
            for(int i = 0; i < scriptables.Count; i++)
            {
                scriptables[i].RuntimeSetup();
            }
            shouldUpdate = false;
        }

#if UNITY_EDITOR
        private void OnValidate() {
            FindAllScriptableObjectsRuntime();
        }
        protected virtual void FindAllScriptableObjectsRuntime()
        {
            UnityEditor.EditorUtility.SetDirty(this);
            scriptables = FindAssetsByType<ScriptableProperty>();
        }

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