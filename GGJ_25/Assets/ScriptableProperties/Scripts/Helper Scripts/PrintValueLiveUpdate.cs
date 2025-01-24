﻿using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace ANT.ScriptableProperties
{
    [AddComponentMenu("ANT/Scriptable Properties/Print Value Live Update")]
    public class PrintValueLiveUpdate : MonoBehaviour
    {
        [SerializeField, Tooltip("Scriptable property value will be printed when the scriptable changes.")]
        private ScriptableProperty _scriptable = null;
        private ScriptablePropertyCast cast;

        Text text = null;
        TextMeshProUGUI textMesh = null;
        public string format = "";

        private void Awake()
        {
            //finding the text components. 
            textMesh = GetComponent<TextMeshProUGUI>();
            text = GetComponent<Text>();

            cast = new ScriptablePropertyCast(_scriptable);
            cast.AddEvent(UpdateText);
        }


        public void UpdateText()
        {
            //print the value of the scriptable object in the text component.    
            if (text)
            {
                text.text = cast.GetString();
            }
            //print the value of the scriptable object in the text mesh component.    
            if (textMesh)
            {
                text.text = cast.GetString();
            }
        }
    }
}

