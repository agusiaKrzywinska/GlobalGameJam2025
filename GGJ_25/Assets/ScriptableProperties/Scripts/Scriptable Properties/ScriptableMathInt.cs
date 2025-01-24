using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CreateAssetMenu(fileName = "New Math Int", menuName = "ANT/Scriptable Properties/Scriptable Number/Math Int")]
    public class ScriptableMathInt : ScriptableInt
    {
        [SerializeField]
        private List<ScriptableValue> values = new List<ScriptableValue>();
        [SerializeField]
        private List<Operator> operations = new List<Operator>();
        public List<ScriptableValue> Values 
        { 
            get 
            {
                return values;
            } 
            set 
            {
                values = value;
            } 
        }
        public List<Operator> Operations { get { return operations; } set { operations = value; } }
        private Operator GetOperatorAt(int position)
        {
            if (position < operations.Count && position >= 0)
                return operations[position];
            Debug.LogError($"Position {position} doesn't exist on {name}");
            return Operator.ADD;
        }
        /// <summary>
        /// Gets a value of operator at the position. 
        /// </summary>
        /// <param name="position">Which element to get the value of.</param>
        /// <returns></returns>
        private int GetValueAt(int position)
        {
            if (position < values.Count && position >= 0)
            {
                if (values[position].cast._property && values[position].cast.type != ScriptableType.BOOL && values[position].cast.type != ScriptableType.STRING && values[position].cast.type != ScriptableType.NONE)
                    return values[position].cast.GetInt();
                return values[position].constant;
            }  
            Debug.LogError($"Position {position} doesn't exist on {name}");
            return 0;
        }

        public ScriptableType GetTypeAt(int position)
        {
            if (position < values.Count && position >= 0 && values[position].cast._property)
                return values[position].cast.type;
            return ScriptableType.INT;
        }

        public void DoOperation()
        {
            int currentResult = GetValueAt(0);
            for (int i = 1; i < values.Count; i++)
            {
                switch (GetOperatorAt(i - 1))
                {
                    case Operator.ADD:
                        currentResult += GetValueAt(i);
                        break;
                    case Operator.SUBTRACT:
                        currentResult -= GetValueAt(i);
                        break;
                    case Operator.MULTIPLY:
                        currentResult *= GetValueAt(i);
                        break;
                    case Operator.DIVIDE:
                        currentResult /= GetValueAt(i);
                        break;
                    case Operator.POWER:
                        currentResult = (int)Mathf.Pow(currentResult, GetValueAt(i));
                        break;
                }
            }

            SetValue(currentResult);
        }

        public override int GetValue()
        {
            DoOperation();
            return base.GetValue();
        }

        public override bool ReferenceSelf(List<ScriptableProperty> pastValues)
        {
            bool state = base.ReferenceSelf(pastValues);
            pastValues.Add(this);
            for (int i = 0; i < values.Count; i++)
            {
                if (state == true) return true;
                if (!values[i].cast._property) continue;

                state = values[i].cast._property.ReferenceSelf(pastValues);
            }
            return state;
        }

        /// <summary>
        /// The int operations that can be preformed. 
        /// </summary>
        public enum Operator
        {
            ADD, SUBTRACT, MULTIPLY, DIVIDE, POWER
        }

        public override string GetEquation()
        {
            string op = "";

            string result = $"{name}[" + values[0].GetEquation();
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].cast._property && (values[i].cast.type == ScriptableType.BOOL || values[i].cast.type == ScriptableType.STRING || values[i].cast.type == ScriptableType.NONE))
                {
                    result += $" value {i + 1} is missing ";
                    continue;
                }
                if (i != 0)
                {
                    switch (GetOperatorAt(i - 1))
                    {
                        case Operator.ADD:
                            op = "+";
                            break;
                        case Operator.SUBTRACT:
                            op = "-";
                            break;
                        case Operator.MULTIPLY:
                            op = "*";
                            break;
                        case Operator.DIVIDE:
                            op = "/";
                            break;
                        case Operator.POWER:
                            op = "^";
                            break;
                    }
                    result += $" {op} {values[i].GetEquation()}";
                }
            }
            return result + "]";
        }

        [System.Serializable]
        public class ScriptableValue
        {
            public ScriptablePropertyCast cast;
            public int constant;
            [HideInInspector]
            public bool hideScriptableField;

            public string GetEquation()
            {
                if(cast._property)
                {
                    return cast._property.GetEquation();
                }
                return constant.ToString();
            }

        }
    }
}
