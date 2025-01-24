using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    //Holder class for Scriptable Property to allow for referencing in editor scripts and organize.
    public abstract class ScriptableProperty : ScriptableObject
    {
        /// <summary>
        /// Will set the value back to whatever was before playmode. 
        /// </summary>
        public abstract void ResetValue();

        protected abstract void OnEnable();

        public void RuntimeSetup()
        {
            OnEnable();
        }

        public abstract void ValueChanged();
        /// <summary>
        /// What the preview equation is for the scriptable object. 
        /// </summary>
        /// <returns>The value in string format to be displayed as an equation.</returns>
        public abstract string GetEquation();
        /// <summary>
        /// This is used to ensure when setting up operations that they don't reference themselves and crash unity.
        /// </summary>
        /// <param name="pastValues">The parent references of the property.</param>
        /// <returns></returns>
        public virtual bool ReferenceSelf(List<ScriptableProperty> pastValues)
        {
            return pastValues.Contains(this);
        }
    }

    public enum ScriptableType { NONE, BOOL, INT, FLOAT, LONG, STRING }

    //Helps for generics to allow for passing similar types together. 
    [System.Serializable]
    public struct ScriptablePropertyCast
    {
        public ScriptableProperty _property;
        [HideInInspector]
        public ScriptableInt _int;
        [HideInInspector]
        public ScriptableFloat _float;
        [HideInInspector]
        public ScriptableBool _bool;
        [HideInInspector]
        public ScriptableString _string;
        [HideInInspector]
        public ScriptableType type;

        /// <summary>
        /// Converts a ScriptableProperty to it's actual type and stores it.
        /// </summary>
        /// <param name="value"></param>
        public ScriptablePropertyCast(ScriptableProperty value)
        {
            _property = value;
            _int = value as ScriptableInt;
            _float = value as ScriptableFloat;
            _bool = value as ScriptableBool;
            _string = value as ScriptableString;

            type = ScriptableType.NONE;
            type = SetType();
        }

        private ScriptableType SetType()
        {
            if (_int)
            {
                return ScriptableType.INT;
            }
            else if (_float)
            {
                return ScriptableType.FLOAT;
            }
            else if (_bool)
            {
                return ScriptableType.BOOL;
            }
            else if (_string)
            {
                return ScriptableType.STRING;
            }
            else
            {
                return ScriptableType.NONE;
            }
        }

        private void ResetProperties()
        {
            _int = null;
            _float = null;
            _bool = null;
            _string = null;
            type = ScriptableType.NONE;
        }
        public void SetProperty(ScriptableProperty value)
        {
            _property = value;
            if (_property == null)
            {
                ResetProperties();
                return;
            }
            _int = value as ScriptableInt;
            _float = value as ScriptableFloat;
            _bool = value as ScriptableBool;
            _string = value as ScriptableString;

            type = SetType();
        }

        public bool GetBool()
        {
            if (_bool)
            {
                return _bool.GetValue();
            }
            else if (_string)
            {
                if (bool.TryParse(_string.GetValue(), out bool temp))
                {
                    return temp;
                }
            }

            Debug.LogError("Unsupported data type!");
            return false;
        }

        public int GetInt()
        {
            if (_int)
            {
                return _int.GetValue();
            }
            else if (_float)
            {
                return (int)_float.GetValue();
            }
            else if (_string)
            {
                if (int.TryParse(_string.GetValue(), out int temp))
                {
                    return temp;
                }
            }

            Debug.LogError("Unsupported data type!");
            return 0;
        }

        public float GetFloat()
        {
            if (_int)
            {
                return _int.GetValue();
            }
            else if (_float)
            {
                return _float.GetValue();
            }
            else if (_string)
            {
                if (float.TryParse(_string.GetValue(), out float temp))
                {
                    return temp;
                }
            }

            Debug.LogError("Unsupported data type!");
            return 0f;
        }

        public string GetString()
        {
            if (_int)
            {
                return _int.ToString();
            }
            else if (_float)
            {
                return _float.ToString();
            }
            else if (_bool)
            {
                return _bool.ToString();
            }
            else if (_string)
            {
                return _string.GetValue();
            }

            Debug.LogError("Unsupported data type!");
            return "";
        }

        public void AddEvent(UnityAction action)
        {
            if (_int)
            {
                _int.OnValueChanged.AddListener((x) => action.Invoke());
            }
            else if (_float)
            {
                _float.OnValueChanged.AddListener((x) => action.Invoke());
            }
            else if (_bool)
            {
                _bool.OnValueChanged.AddListener((x) => action.Invoke());
            }
            else if (_string)
            {
                _string.OnValueChanged.AddListener((x) => action.Invoke());
            }
            else
            {
                //error. 
            }
        }

        public UnityEvent<bool> OnBoolValueChanged()
        {
            return _bool.OnValueChanged;
        }
        public UnityEvent<float> OnFloatValueChanged()
        {
            return _float.OnValueChanged;
        }
        public UnityEvent<int> OnIntValueChanged()
        {
            return _int.OnValueChanged;
        }
        public UnityEvent<string> OnStringValueChanged()
        {
            return _string.OnValueChanged;
        }
    }
}