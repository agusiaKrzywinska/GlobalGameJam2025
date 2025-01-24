using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New String", menuName = "ANT/Scriptable Properties/Scriptable String/String")]
    public class ScriptableString : ScriptablePropertyGeneric<string>
    {
        /// <summary>
        /// This updates the value to the scriptable property that is being passed through. 
        /// </summary>
        /// <param name="newValue">The new value that it is being set to.</param>
        public void SetValue(ScriptableString newValue)
        {
            if (!newValue) return;
            value = newValue.GetValue();
        }
        
        /// <summary>
        /// Adds the passthrough string to the start of string value. 
        /// </summary>
        public void AddAtBeginning(string newBeginning)
        {
            SetValue(newBeginning + GetValue());
        }

        /// <summary>
        /// Adds the passthrough string to the start of string value. 
        /// </summary>
        public void AddAtBeginning(ScriptableString newBeginning)
        {
            if (!newBeginning) return;
            AddAtBeginning(newBeginning.GetValue());
        }

        /// <summary>
        /// Adds the passthrough string to the end of string value. 
        /// </summary>
        public void AddAtEnd(string newEnd)
        {
            SetValue(GetValue() + newEnd);
        }

        /// <summary>
        /// Adds the passthrough string to the end of string value. 
        /// </summary>
        public void AddAtEnd(ScriptableString newEnd)
        {
            if (!newEnd) return;
            AddAtEnd(newEnd.GetValue());
        }

        /// <summary>
        /// Performs the ToUpper string operation. 
        /// </summary>
        public void ToUpper()
        {
            SetValue(GetValue().ToUpper());
        }

        /// <summary>
        /// Performs the ToLower string operation.
        /// </summary>
        public void ToLower()
        {
            SetValue(GetValue().ToLower());
        }
    }
}