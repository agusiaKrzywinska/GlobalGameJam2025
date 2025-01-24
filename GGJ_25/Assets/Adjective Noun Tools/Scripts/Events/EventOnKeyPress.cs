using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    public class EventOnKeyPress : MonoBehaviour
    {
        [SerializeField, Tooltip("The key code you wish to press to trigger all the unity events.")]
        private KeyCode keyPressed;
        [SerializeField, Tooltip("When the key is pressed down for the first frame this unity event will fire.")]
        private UnityEvent onKeyDown;
        [SerializeField, Tooltip("while the key is pressed down this unity event will fire.")]
        private UnityEvent onKeyPress;
        [SerializeField, Tooltip("When the key is released from being pressed for the first frame this unity event will fire.")]
        private UnityEvent onKeyUp;

        private void Update()
        {
            if(Input.GetKeyDown(keyPressed))
            {
                onKeyDown.Invoke();
            }
            
            if(Input.GetKey(keyPressed))
            {
                onKeyPress.Invoke();
            }

            if(Input.GetKeyUp(keyPressed))
            {
                onKeyUp.Invoke();
            }
        }

    }
}