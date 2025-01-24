using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.ScriptableProperties;

namespace ANT.EzTalk
{
    [System.Serializable]
    public class EzTalkParameter 
    {
        public Parameter parameterType;
        public string name;
        [SerializeField]
        private int _int;
        public int Int => _int;
        [SerializeField]
        private float _float;
        public float Float => _float;
        [SerializeField]
        private bool _bool;
        public bool Bool => _bool;
        [SerializeField]
        private string _string;
        public string String => _string;
        [SerializeField]
        private ScriptableProperty scriptableProperty;
        public ScriptableProperty ScriptableProperty {get=> scriptableProperty; set => scriptableProperty = value;}
        public EzTalkParameter(Parameter type)
        {
            parameterType = type;
        }
        public void SetValue(int value)
        {
            if(parameterType == Parameter.Int)
                _int = value;
        }

        public void SetValue(bool value)
        {
            if (parameterType == Parameter.Bool)
                _bool = value;
        }

        public void SetValue(string value)
        {
            if (parameterType == Parameter.String)
                _string = value;
        }

        public void SetValue(float value)
        {
            if (parameterType == Parameter.Float)
                _float = value;
        }
        public string GetValue()
        {
            switch(parameterType)
            {
                case Parameter.Int:
                    return _int.ToString();
                case Parameter.Bool:
                    return _bool.ToString();
                case Parameter.String:
                    return _string;
                case Parameter.Float:
                    return _float.ToString();
                case Parameter.ScriptableProperty:
                    if (!scriptableProperty) return "";
                return scriptableProperty.ToString();
            }
            return "";
        }

        public override string ToString()
        {
            return GetValue();
        }
    }
}
