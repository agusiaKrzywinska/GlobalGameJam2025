using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    [CreateAssetMenu(fileName ="Scriptable Event", menuName = "ANT/Scriptable Properties/Scriptable Event")]
    public class ScriptableUnityEvent : ScriptableObject
    {
        [SerializeField, Tooltip("")]
        private UnityEvent eventsToRun;
        
        /// <summary>
        /// all the listeners that are registered to this scriptable object. 
        /// </summary>
        private List<ScriptableUnityEventListener> listeners = new List<ScriptableUnityEventListener>();
        

        /// <summary>
        /// Fire this event as well as all of its listeners. 
        /// </summary>
        public void RunEvents()
        {
            eventsToRun.Invoke();

            for(int i = 0; i < listeners.Count; i++)
            {
                if(listeners[i])
                {
                    listeners[i].RunEvents();
                }
            }
        }

        /// <summary>
        /// Register a listener to the this object. 
        /// </summary>
        /// <param name="newListener">The listener to register.</param>
        [HideInInspector]
        public void AddListener(ScriptableUnityEventListener newListener)
        {
            if(!listeners.Contains(newListener))
            {
                listeners.Add(newListener);
            }
        }

        /// <summary>
        /// Deregister a listener to the this object. 
        /// </summary>
        /// <param name="newListener">The listener to deregister.</param>
        [HideInInspector]
        public void RemoveListener(ScriptableUnityEventListener deletedListener)
        {
            if(listeners.Contains(deletedListener))
            {
                listeners.Remove(deletedListener);
            }
        }

    }
}