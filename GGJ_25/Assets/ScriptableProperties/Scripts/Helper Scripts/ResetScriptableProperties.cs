using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [AddComponentMenu("Adjective Noun Tools/Scriptable Properties/Reset Scriptable Properties")]
    public class ResetScriptableProperties : MonoBehaviour
    {
        [SerializeField]
        private ScriptableProperty[] propertiesToReset = null;
        [SerializeField]
        private bool resetOnEnable = false;

        private void OnEnable()
        {
            if (resetOnEnable)
                ResetAllProperties();
        }

        public void ResetAllProperties()
        {
            for(int i = 0; i < propertiesToReset.Length; i++)
            {
                if(propertiesToReset[i])
                    propertiesToReset[i].ResetValue();
            }
        }

    }
}