using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "New String Operator", menuName = "ANT/Scriptable Properties/Scriptable String/Operator String")]
    public class ScriptableOperatorString : ScriptableOperator
    {
        [SerializeField]
        private ScriptableString value1;
        public Operator operation;
        [SerializeField]
        private ScriptableString value2;
        [SerializeField]
        private string value2Constant;
        public ScriptableString Value1 { get => value1; set { value1 = value; } }
        public ScriptableString Value2 { get => value2; set { value2 = value; } }
        public string Value2Constant { get => value2Constant; set { value2Constant = value; } }

        protected override void OnEnable()
        {
            base.OnEnable();
            //when the children change tell the parent that they changed. 
            if (Application.isPlaying)
            {
                if(value1)
                    value1.OnValueChanged.AddListener(x => ValueChanged());
                if(value2)
                    value2.OnValueChanged.AddListener(x => ValueChanged());
            }
        }
        
        /// <returns>The value of the second operator in string format to be displayed as an equation of the proper type.</returns>
        private string GetValue2()
        {
            if (value2)
                return value2.GetValue();
            return value2Constant;
        }
        public override void DoOperation()
        {
            if (!value1)
            {
                Debug.LogError($"{name} does not have value 1 set");
                return;
            }

            switch (operation)
            {
                case Operator.EQUAL_TO:
                    SetValue(value1.GetValue() == GetValue2());
                    break;
                case Operator.NOT_EQUAL:
                    SetValue(value1.GetValue() != GetValue2());
                    break;
                case Operator.CONTAINS:
                    SetValue(value1.GetValue().Contains(GetValue2()));
                    break;
                case Operator.NOT_CONTAINS:
                    SetValue(!value1.GetValue().Contains(GetValue2()));
                    break;
            }
        }
        
        /// <summary>
        /// The string operations that can be performed. 
        /// </summary>
        public enum Operator
        {
            EQUAL_TO, NOT_EQUAL, CONTAINS, NOT_CONTAINS
        }

        public override string GetEquation()
        {
            if(!value1)
                return $"{name}[Missing Value 1]";

            string op = "";
            switch (operation)
            {
                case Operator.EQUAL_TO:
                    op = "==";
                    break;
                case Operator.NOT_EQUAL:
                    op = "!=";
                    break;
                case Operator.CONTAINS:
                    op = " contains ";
                    break;
                case Operator.NOT_CONTAINS:
                    op = " !contains ";
                    break;
            }
            if (value2)
                return $"{name}[{value1.GetEquation()} {op} {value2.GetEquation()}]";
            return $"{name}[{value1.GetEquation()} {op} {value2Constant}]";
        }
    }
}
