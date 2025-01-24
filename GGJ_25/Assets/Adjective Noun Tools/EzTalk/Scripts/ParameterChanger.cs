using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.EzTalk
{
    public class ParameterChanger : MonoBehaviour
    {
        public Dialogue dialogue;
        public string parameter;

        public void SetBool(bool value)
        {
            if(dialogue)
                dialogue.SetBool(parameter, value);
        }
        
        public void SetString(string value)
        {
            if (dialogue)
                dialogue.SetString(parameter, value);
        }

        public void SetFloat(float value)
        {
            if (dialogue)
                dialogue.SetFloat(parameter, value);
        }

        public void SetInt(int value)
        {
            if (dialogue)
                dialogue.SetInt(parameter, value);
        }
    }
}