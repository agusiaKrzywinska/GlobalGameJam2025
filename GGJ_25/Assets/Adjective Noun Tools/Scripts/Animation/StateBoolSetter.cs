using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.ScriptableProperties;

namespace ANT
{
    public class StateBoolSetter : StateMachineBehaviour
    {
        //is public to allow for setting local instances in other scripts. 
        [Tooltip("A scriptable bool value you want to change when it it enters and exits this state.")]
        public ScriptableBool resultToChange;
        [SerializeField, Tooltip("The value it will set when it enters the state.")]
        private bool onEnterState = true;
        [SerializeField, Tooltip("The value it will set when it exits the state.")]
        private bool onExitState = false;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            resultToChange.SetValue(onEnterState);
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            resultToChange.SetValue(onExitState);
        }
    }
}