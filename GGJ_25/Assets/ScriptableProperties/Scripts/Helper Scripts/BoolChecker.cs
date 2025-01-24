using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    [AddComponentMenu("ANT/Scriptable Properties/Bool Checker")]
    public class BoolChecker : MonoBehaviour
    {
        [SerializeField, Tooltip("The scriptable bool that will be checked to decide the outcome.")]
        private ScriptableBool condition = null;
        [SerializeField, Tooltip("Events that will run if the bool is true")]
        private UnityEvent onTrue = null;
        [SerializeField, Tooltip("Events that will run if the bool is false")]
        private UnityEvent onFalse = null;
        /// <summary>
        /// Checks the value of the scriptable bool and runs unity events based off of it. 
        /// </summary>
        public void CheckCondition()
        {
            if (!condition) Debug.LogError($"Missing scriptable bool on {name}", this);
            if(condition.GetValue())
            {
                onTrue.Invoke();
            }
            else
            {
                onFalse.Invoke();
            }
        }
    }
}
