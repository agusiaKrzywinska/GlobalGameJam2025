using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    public class EventOnMouse : MonoBehaviour
    {
        [SerializeField, Tooltip("Unity Events are fired when you click on the gameobject on the first frame with this script attached.")]
        public UnityEvent onMouseDown;
        [SerializeField, Tooltip("Unity Events are fired when you mouse over this gameobject for the first frame of entering with this script attached.")]
        private UnityEvent onMouseEnter;
        
        [SerializeField, Tooltip("Unity Events are fired when you mouse out this gameobject for the first frame of exiting with this script attached.")]
        private UnityEvent onMouseExit;
        
        [SerializeField, Tooltip("Unity Events are fired when you release from a click on the gameobject on the first frame with this script attached.")]
        private UnityEvent onMouseUp;
        [SerializeField, Tooltip("Unity Events are fired when you release from a click on the gameobject on the first frame with this script attached. This must be released over the same collider as pressed.")]
        private UnityEvent onMouseUpAsButton;
        private void OnMouseDown()
        {
            if (TouchHelper.TouchOnUI())
                return;

            onMouseDown.Invoke();
        }

        private void OnMouseEnter()
        {
            if (TouchHelper.TouchOnUI())
                return;

            onMouseEnter.Invoke();
        }

        private void OnMouseExit()
        {
            if (TouchHelper.TouchOnUI())
                return;

            onMouseExit.Invoke();
        }

        private void OnMouseUp()
        {
            if (TouchHelper.TouchOnUI())
                return;

            onMouseUp.Invoke();
        }

        private void OnMouseUpAsButton()
        {
            if (TouchHelper.TouchOnUI())
                return;

            onMouseUpAsButton.Invoke();
        }
    }
}