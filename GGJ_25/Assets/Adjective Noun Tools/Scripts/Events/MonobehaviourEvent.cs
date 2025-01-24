using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    [AddComponentMenu("ANT/Scriptable Properties/Monobehaviour Event")]
    public class MonobehaviourEvent : MonoBehaviour
    {
        [SerializeField, Tooltip("The unity event to run on awake.")]
        private UnityEvent onAwake;

        [SerializeField, Tooltip("The unity event to run on start.")]
        private UnityEvent onStart;
        [SerializeField, Tooltip("The unity event to run on enable.")]
        private UnityEvent onEnable;
        [SerializeField, Tooltip("The unity event to run on disable.")]
        private UnityEvent onDisable;
        [SerializeField, Tooltip("The unity event to run on validate if run on validate is true.")]
        private UnityEvent onValidate;

        [SerializeField, Tooltip("Will allow for the the onValidate to run if true.")]
        private bool runOnValidate = false;
        private void Awake()
        {
            if (onAwake != null)
                onAwake.Invoke();
        }
        // Start is called before the first frame update
        void Start()
        {
            if (onStart != null)
                onStart.Invoke();
        }

        private void OnEnable()
        {
            if (onEnable != null)
                onEnable.Invoke();
        }

        private void OnDisable()
        {
            if (onDisable != null)
                onDisable.Invoke();
        }

        private void OnValidate()
        {
            if (onValidate != null && runOnValidate)
            {
                onValidate.Invoke();
                runOnValidate = false;
                Debug.Log("Ran events");
            }
        }
    }
}