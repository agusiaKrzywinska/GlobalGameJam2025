using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    public class EventOnButton : MonoBehaviour
    {
        [SerializeField, Tooltip("The unity event to run when you press the button.")]
        private UnityEvent _event;

        [NaughtyAttributes.Button()]
        public void RunEvent()
        {
            _event.Invoke();
        }

    }
}