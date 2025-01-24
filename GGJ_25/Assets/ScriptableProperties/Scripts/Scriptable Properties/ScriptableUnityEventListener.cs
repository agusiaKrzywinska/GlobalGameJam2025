using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    public class ScriptableUnityEventListener : MonoBehaviour
    {
        [SerializeField, Tooltip("")]
        ScriptableUnityEvent myScriptableEvent;
        [SerializeField, Tooltip("")]
        UnityEvent myEvents;

        private void OnEnable()
        {
            //register the listener to the scriptable object. 
            if (myScriptableEvent)
                myScriptableEvent.AddListener(this);
        }

        private void OnDisable()
        {
            // deregister the listener to the scriptable object. 
            if (myScriptableEvent)
                myScriptableEvent.RemoveListener(this);
        }

        /// <summary>
        /// Fire the events associated to the listener. 
        /// </summary>
        public void RunEvents()
        {
            myEvents.Invoke();
        }
    }
}