using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Number Operator", menuName = "ANT/Scriptable Properties/Scriptable Number/Operator Number")]
    public class ScriptableOperatorNumber : ScriptableOperator
    {
        [SerializeField]
        private ScriptablePropertyCast value1;
        public Operator operation;
        [SerializeField]
        private ScriptablePropertyCast value2;
        [SerializeField]
        private int value2IntConstant;
        [SerializeField]
        private float value2FloatConstant;
        [SerializeField]
        private long value2LongConstant;

        public int Value2ConstantInt { get => value2IntConstant; set { value2IntConstant = value; } }
        public float Value2ConstantFloat { get => value2FloatConstant; set { value2FloatConstant = value; } }
        public long Value2ConstantLong { get => value2LongConstant; set { value2LongConstant = value; } }

        public ScriptableType GetTypeOfCast { get => value1.type; }
        public ScriptableType GetTypeOfValue2 { get => value2.type; }
        public ScriptableProperty Value1
        {
            get
            {
                return value1._property;
            }
            set
            {
                value1 = new ScriptablePropertyCast(value);
            }
        }

        public ScriptableProperty Value2
        {
            get
            {
                return value2._property;
            }
            set
            {
                value2 = new ScriptablePropertyCast(value);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            //when the children change tell the parent that they changed. 
            if (Application.isPlaying)
            {
                if (Value1)
                    AddValueChangedListeners(value1);
                if (Value2)
                    AddValueChangedListeners(value2);;
            }
        }

        private void AddValueChangedListeners(ScriptablePropertyCast cast)
        {
            switch(cast.type)
            {
                case ScriptableType.INT:
                    cast.OnIntValueChanged().AddListener((x)=> ValueChanged());
                    break;
                case ScriptableType.FLOAT:
                    cast.OnFloatValueChanged().AddListener((x)=> ValueChanged());
                    break;
            }

        }

        /// <returns>The value of the second operator in string format to be displayed as an equation of the proper type.</returns>
        private string GetValue2Equation(ScriptableType type)
        {
            if (value2._property)
                return value2._property.GetEquation();
            switch (type)
            {
                case ScriptableType.INT:
                    return value2IntConstant.ToString();
                case ScriptableType.FLOAT:
                    return value2FloatConstant.ToString();
                case ScriptableType.LONG:
                    return value2LongConstant.ToString();
            }
            return "Value 2 not found";
        }

        private int GetValue2Int()
        {
            if(!value2._property)
            {
                return value2IntConstant;
            }

            if (value2.type == ScriptableType.NONE || value2.type == ScriptableType.BOOL || value2.type == ScriptableType.STRING)
            {
                return value2IntConstant;
            }
            return value2.GetInt();
        }

        private float GetValue2Float()
        {
            if (!value2._property)
            {
                return value2FloatConstant;
            }

            if (value2.type == ScriptableType.NONE || value2.type == ScriptableType.BOOL || value2.type == ScriptableType.STRING)
            {
                return value2FloatConstant;
            }
            return value2.GetFloat();
        }

        public override void DoOperation()
        {
            if (value1.type == ScriptableType.NONE || value1.type == ScriptableType.BOOL || value1.type == ScriptableType.STRING)
            {
                Debug.LogError($"{name} does not have value 1 set");
                return;
            }


            if(value1.type == ScriptableType.INT)
            {
                switch (operation)
                {
                    case Operator.EQUAL_TO:
                        SetValue(value1.GetInt() == GetValue2Int());
                        break;
                    case Operator.NOT_EQUAL:
                        SetValue(value1.GetInt() != GetValue2Int());
                        break;
                    case Operator.LESS_THAN:
                        SetValue(value1.GetInt() < GetValue2Int());
                        break;
                    case Operator.LESS_THAN_OR_EQUAL_TO:
                        SetValue(value1.GetInt() <= GetValue2Int());
                        break;
                    case Operator.GREATER_THAN:
                        SetValue(value1.GetInt() > GetValue2Int());
                        break;
                    case Operator.GREATER_THAN_OR_EQUAL_TO:
                        SetValue(value1.GetInt() >= GetValue2Int());
                        break;
                }
            }
            else if (value1.type == ScriptableType.FLOAT)
            {
                switch (operation)
                {
                    case Operator.EQUAL_TO:
                        SetValue(value1.GetFloat() == GetValue2Float());
                        break;
                    case Operator.NOT_EQUAL:
                        SetValue(value1.GetFloat() != GetValue2Float());
                        break;
                    case Operator.LESS_THAN:
                        SetValue(value1.GetFloat() < GetValue2Float());
                        break;
                    case Operator.LESS_THAN_OR_EQUAL_TO:
                        SetValue(value1.GetFloat() <= GetValue2Float());
                        break;
                    case Operator.GREATER_THAN:
                        SetValue(value1.GetFloat() > GetValue2Float());
                        break;
                    case Operator.GREATER_THAN_OR_EQUAL_TO:
                        SetValue(value1.GetFloat() >= GetValue2Float());
                        break;
                }
            }
        }
        
        /// <summary>
        /// The number operations that can be performed. 
        /// </summary>
        public enum Operator
        {
            EQUAL_TO, NOT_EQUAL, LESS_THAN, LESS_THAN_OR_EQUAL_TO, GREATER_THAN, GREATER_THAN_OR_EQUAL_TO
        }

        public override string GetEquation()
        {
            if (value1.type == ScriptableType.NONE || value1.type == ScriptableType.BOOL || value1.type == ScriptableType.STRING)
            {
                return $"{name}[Missing Value 1]";
            }

            string op = "";
            switch (operation)
            {
                case Operator.EQUAL_TO:
                    op = "==";
                    break;
                case Operator.NOT_EQUAL:
                    op = "!=";
                    break;
                case Operator.LESS_THAN:
                    op = "<";
                    break;
                case Operator.LESS_THAN_OR_EQUAL_TO:
                    op = "<=";
                    break;
                case Operator.GREATER_THAN:
                    op = ">";
                    break;
                case Operator.GREATER_THAN_OR_EQUAL_TO:
                    op = ">=";
                    break;
            }

            return $"{name}[{value1._property.GetEquation()} {op} {GetValue2Equation(value1.type)}]";
        }
    }
}
