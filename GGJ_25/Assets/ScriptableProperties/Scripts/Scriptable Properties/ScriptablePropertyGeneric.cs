using UnityEngine;
using UnityEngine.Events;

namespace ANT.ScriptableProperties
{
    public class ScriptablePropertyGeneric<T> : ScriptableProperty
    {
        
        [SerializeField, Tooltip("")]
        protected UnityEvent<T> onValueChanged;

        public virtual UnityEvent<T> OnValueChanged
        {
            get => onValueChanged;
        }

        [SerializeField, NaughtyAttributes.Label("Value"), NaughtyAttributes.ShowIf(nameof(isPlaying)), Tooltip("")]
        protected T value;
        [SerializeField,NaughtyAttributes.Label("Value"), NaughtyAttributes.ShowIf(nameof(isInEditorMode)), Tooltip("")]
        protected T defaultValue;

        protected bool isPlaying => Application.isPlaying;
        protected bool isInEditorMode => Application.isPlaying == false;

        ///<summary>
        /// Will invoke the events for the changed value. 
        ///</summary>
        public override void ValueChanged()
        {
            if (onValueChanged != null)
                onValueChanged.Invoke(value);
        }

        public virtual void ValueChanged(T result)
        {
            if (onValueChanged != null)
                onValueChanged.Invoke(result);
        }

        /// <summary>
        /// Resetting the values when it runs for the first time
        /// </summary>
        protected override void OnEnable()
        {
            value = defaultValue;
        }

        /// <summary>
        /// This will reset the value to the default value set in editor mode. 
        /// </summary>
        public override void ResetValue()
        {
            SetValue(defaultValue);
        }

        /// <summary>
        /// Updates the TextMeshProUI component to be whatever the value is. 
        /// </summary>
        public void PrintValue(TMPro.TextMeshProUGUI textMesh)
        {
            textMesh.text = GetString();
        }
        /// <summary>
        /// Updates the TextMeshPro component to be whatever the value is. 
        /// </summary>
        public void PrintValue(TMPro.TextMeshPro textMesh)
        {
            textMesh.text = GetString();
        }
        /// <summary>
        /// Updates the Text component to be whatever the value is. 
        /// </summary>
        public void PrintValue(UnityEngine.UI.Text text)
        {
            text.text = GetString();
        }

        /// <summary>
        /// Updates the value to the scriptable property that is being passed through. 
        /// </summary>
        /// <param name="newValue">The new value that it is being set to.</param>
        public virtual void SetValue(T newValue)
        {
            if (!Application.isPlaying)
            {
                defaultValue = newValue;
            }
            if (!value.Equals(newValue))
            {
                value = newValue;
                ValueChanged();
            }
        }

        /// <returns>The current value in the scriptable property.</returns>
        public virtual T GetValue()
        {
            if (!Application.isPlaying)
            {
                return defaultValue;
            }
            return value;
        }

        /// <returns>The value in string format.</returns>
        public string GetString()
        {
            return GetValue().ToString();
        }

        public override string GetEquation()
        {
            return name + "[" + GetString() + "]";
        }
    }

}
