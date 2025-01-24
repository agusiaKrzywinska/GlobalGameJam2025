using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Int", menuName = "ANT/Scriptable Properties/Scriptable Number/Int")]
    public class ScriptableInt : ScriptablePropertyGeneric<int>
    {
        /// <summary>
        /// performs the += operation on the value.
        /// </summary>
        public virtual void Add(int amount)
        {
            SetValue(value + amount);
        }

        /// <summary>
        /// performs the -= operation on the value.
        /// </summary>
        public virtual void Subtract(int amount)
        {
            SetValue(value - amount);
        }

        /// <summary>
        /// performs the *= operation on the value.
        /// </summary>
        public virtual void Multiply(int amount)
        {
            SetValue(value * amount);
        }

        /// <summary>
        /// performs the /= operation on the value.
        /// </summary>
        public virtual void Divide(int amount)
        {
            if (amount == 0)
            {
                Debug.LogError($"{name} is attempting to divide by zero");
                return;
            }
            SetValue(value / amount);
        }

        /// <summary>
        /// Updates the value to the scriptable property that is being passed through. 
        /// </summary>
        /// <param name="newValue">The new value that it is being set to.</param>
        public virtual void SetValue(ScriptableProperty newValue)
        {
            if (!newValue) return;
            ScriptablePropertyCast cast = new ScriptablePropertyCast(newValue);
            SetValue(cast.GetInt());
        }

        /// <summary>
        /// This attempts to set a string value to the scriptable property value.
        /// </summary>
        public void SetValue(string value)
        {
            int temp;
            if (int.TryParse(value, out temp))
            {
                SetValue(temp);
            }
            else
            {
                Debug.LogError("Attempting to convert incorrect data type", this);
            }
        }

        /// <summary>
        /// performs the += operation on the value.
        /// </summary>
        public virtual void Add(ScriptableProperty amount)
        {
            if (!amount) return;
            ScriptablePropertyCast cast = new ScriptablePropertyCast(amount);
            Add(cast.GetInt());
        }

        /// <summary>
        /// performs the -= operation on the value.
        /// </summary>
        public virtual void Subtract(ScriptableProperty amount)
        {
            if (!amount) return;
            ScriptablePropertyCast cast = new ScriptablePropertyCast(amount);
            Subtract(cast.GetInt());
        }

        /// <summary>
        /// performs the *= operation on the value.
        /// </summary>
        public virtual void Multiply(ScriptableProperty amount)
        {
            if (!amount) return;
            ScriptablePropertyCast cast = new ScriptablePropertyCast(amount);
            Multiply(cast.GetInt());
        }

        /// <summary>
        /// performs the /= operation on the value.
        /// </summary>
        public virtual void Divide(ScriptableProperty amount)
        {
            if (!amount) return;
            ScriptablePropertyCast cast = new ScriptablePropertyCast(amount);
            Divide(cast.GetInt());
        }
    }
}