using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Bool", menuName = "ANT/Scriptable Properties/Scriptable Bool/Bool")]
    public class ScriptableBool : ScriptablePropertyGeneric<bool>
    {
        /// <summary>
        /// Sets the value to be the passthrough value. 
        /// </summary>
        public void SetValue(ScriptableBool newValue)
        {
            if (!newValue) return;
            SetValue(newValue.GetValue());
        }
        
        /// <summary>
        /// This attempts to set a string value to the scriptable property value.
        /// </summary>
        public void SetValue(string value)
        {
            if(bool.TryParse(value, out bool temp))
            {
                SetValue(temp);
            }
            else
            {
                Debug.LogError("Attempting to convert incorrect data type", this);
            }
        }

        /// <summary>
        /// Used to flip the the current value of the bool. 
        /// </summary>
        public void Toggle()
        {
            SetValue(!value);
        }
    }
}