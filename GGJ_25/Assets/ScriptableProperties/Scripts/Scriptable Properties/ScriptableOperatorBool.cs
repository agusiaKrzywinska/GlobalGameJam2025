using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CreateAssetMenu(fileName = "New Bool Operator", menuName = "ANT/Scriptable Properties/Scriptable Bool/Operator Bool")]
    public class ScriptableOperatorBool : ScriptableOperator
    {
        [SerializeField]
        private List<ScriptableBool> values = new List<ScriptableBool>();
        [SerializeField]
        private List<Operator> operations = new List<Operator>();
        [SerializeField]
        private bool valueConstant;
        public List<ScriptableBool> Values { get { return values; } set { values = value; } }
        public List<Operator> Operations { get { return operations; } set { operations = value; } }
        public bool ValueConstant { get => valueConstant; set { valueConstant = value; } }
        protected override void OnEnable()
        {
            base.OnEnable();
            //when the children change tell the parent that they changed. 
            if(Application.isPlaying)
            {
                for(int i = 0; i < values.Count; i++)
                {
                    values[i].OnValueChanged.AddListener(ValueChanged);
                }
            }
        }

        private Operator GetOperatorAt(int position)
        {
            if (position < operations.Count && position >= 0)
                return operations[position];
            Debug.LogError($"Position {position} doesn't exist on {name}");
            return Operator.AND;
        }

        /// <summary>
        /// Gets a value of operator at the position. 
        /// </summary>
        /// <param name="position">Which element to get the value of.</param>
        /// <returns></returns>
        private bool GetValueAt(int position)
        {
            if (position < values.Count && position >= 0 && values[position])
                return values[position].GetValue();
            Debug.LogError($"Position {position} doesn't exist on {name}");
            return false;
        }
        public override void DoOperation()
        {
            if (values.Count <= 0 || !values[0])
            {
                Debug.LogError($"{name} does not have value 1 set");
                return;
            }

            if (values.Count > 1)
            {
                bool currentResult = GetValueAt(0);
                for (int i = 1; i < values.Count; i++)
                {
                    switch (GetOperatorAt(i-1))
                    {
                        case Operator.EQUAL_TO:
                            currentResult = currentResult == GetValueAt(i);
                            break;
                        case Operator.NOT_EQUAL:
                            currentResult = currentResult != GetValueAt(i);
                            break;
                        case Operator.AND:
                            currentResult &= GetValueAt(i);
                            break;
                        case Operator.OR:
                            currentResult |= GetValueAt(i);
                            break;
                    }
                }

                SetValue(currentResult);
            }
            else
            {
                switch (GetOperatorAt(0))
                {
                    case Operator.EQUAL_TO:
                        SetValue(valueConstant == GetValueAt(0));
                        break;
                    case Operator.NOT_EQUAL:
                        SetValue(valueConstant != GetValueAt(0));
                        break;
                    case Operator.AND:
                        SetValue(valueConstant && GetValueAt(0));
                        break;
                    case Operator.OR:
                        SetValue(valueConstant || GetValueAt(0));
                        break;
                }
            }

        }

        public override bool ReferenceSelf(List<ScriptableProperty> pastValues)
        {
            bool state = base.ReferenceSelf(pastValues);
            pastValues.Add(this);
            for(int i = 0; i < values.Count; i++)
            {
                if (state == true) return true;
                if (!values[i]) continue;

                state = values[i].ReferenceSelf(pastValues);
            }
            return state;
        }

        /// <summary>
        /// The bool operations that can be preformed. 
        /// </summary>
        public enum Operator
        {
            EQUAL_TO, NOT_EQUAL, AND, OR
        }

        public override string GetEquation()
        {
            if (values.Count <= 0 || !values[0])
                return $"{name}[Missing Value 1]";

            string op = "";

            string result = $"{name}[" + values[0].GetEquation();
            if(values.Count == 1)
            {
                switch (GetOperatorAt(0))
                {
                    case Operator.EQUAL_TO:
                        op = "==";
                        break;
                    case Operator.NOT_EQUAL:
                        op = "!=";
                        break;
                    case Operator.AND:
                        op = "&&";
                        break;
                    case Operator.OR:
                        op = "||";
                        break;
                }
                return result + $" {op} {valueConstant}]";
            }
            
            for(int i = 1; i < values.Count; i++)
            {
                if(!values[i])
                {
                    result += $"value {i+1} is missing";
                    continue;
                }
                switch (GetOperatorAt(i-1))
                {
                    case Operator.EQUAL_TO:
                        op = "==";
                        break;
                    case Operator.NOT_EQUAL:
                        op = "!=";
                        break;
                    case Operator.AND:
                        op = "&&";
                        break;
                    case Operator.OR:
                        op = "||";
                        break;
                }
                result += $" {op} {values[i].GetEquation()}";
            }
            return result + "]";
        }
    }
}
