using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Int Range", menuName = "ANT/Scriptable Properties/Scriptable Number/Int Range")]
    public class ScriptableIntRange : ScriptableInt, IRange<int>
    {
        [SerializeField]
        private ScriptableInt min = null;
        [NaughtyAttributes.Label("Min Constant Value"), NaughtyAttributes.ShowIf(nameof(showMinRuntime))]
        [SerializeField, Tooltip("")]
        private int minConstant;
        [NaughtyAttributes.Label("Min Constant Value"), NaughtyAttributes.ShowIf(nameof(showMinEditor))]
        [SerializeField, Tooltip("")]
        private int defaultMinConstant;
        [SerializeField]
        private ScriptableInt max = null;
        [NaughtyAttributes.Label("Max Constant Value"), NaughtyAttributes.ShowIf(nameof(showMaxRuntime))]
        [SerializeField, Tooltip("")]
        private int maxConstant;
        [NaughtyAttributes.Label("Max Constant Value"), NaughtyAttributes.ShowIf(nameof(showMaxEditor))]
        [SerializeField, Tooltip("")]
        private int defaultMaxConstant;

        protected bool showMinRuntime => min == null && isPlaying;
        protected bool showMinEditor => min == null && isInEditorMode;
        protected bool showMaxRuntime => max == null && isPlaying;
        protected bool showMaxEditor => max == null && isInEditorMode;

        protected override void OnEnable()
        {
            base.OnEnable();
            minConstant = defaultMinConstant;
            maxConstant = defaultMaxConstant;
        }

        public override void SetValue(int newValue)
        {
            base.SetValue(Mathf.Clamp(newValue, GetMin(), GetMax()));
        }

        public void KeepInRange()
        {
            SetValue(Mathf.Clamp(value, GetMin(), GetMax()));
        }

        public override string GetEquation()
        {
            return name + "[" + GetValue().ToString() + "]";
        }

        public int GetMin()
        {
            if (min)
            {
                return min.GetValue();
            }
            return minConstant;
        }

        public int GetMax()
        {
            if (max)
            {
                return max.GetValue();
            }
            return maxConstant;
        }

        public void SetMin(int value)
        {
            if (min)
            {
                min.SetValue(value);
            }
            else
            {
                minConstant = value;
                if (!Application.isPlaying)
                    defaultMinConstant = value;
            }
            SwapMinMax();
            KeepInRange();
        }

        public void SetMax(int value)
        {
            if (max)
            {
                max.SetValue(value);
            }
            else
            {
                maxConstant = value;
                if (!Application.isPlaying)
                    defaultMaxConstant = value;
            }
            SwapMinMax();
            KeepInRange();
        }

        /// <summary>
        /// Changes the min value
        /// </summary>
        public void SetMin(ScriptableInt value)
        {
            if (!value) return;
            SetMin(value.GetValue());
        }

        /// <summary>
        /// Changes the max value
        /// </summary>
        public void SetMax(ScriptableInt value)
        {
            if (!value) return;
            SetMax(value.GetValue());
        }

        public void SwapMinMax()
        {
            if (GetMin() > GetMax())
            {
                int temp = GetMax();
                SetMax(GetMin());
                SetMin(temp);
            }
        }
    }
}