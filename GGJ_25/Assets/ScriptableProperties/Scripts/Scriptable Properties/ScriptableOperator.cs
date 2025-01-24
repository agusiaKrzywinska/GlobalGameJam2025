using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    public abstract class ScriptableOperator : ScriptableBool
    {
        /// <summary>
        /// Performs the operation 
        /// </summary>
        public abstract void DoOperation();
        public override bool GetValue()
        {
            DoOperation();
            return base.GetValue();
        }
    }
}