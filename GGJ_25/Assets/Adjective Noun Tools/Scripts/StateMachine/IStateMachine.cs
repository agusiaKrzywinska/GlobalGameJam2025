using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ANT
{
    public interface IStateMachine
    {
        /// <summary>
        /// All states associated to this state machine.
        /// </summary>
        State[] MyStates { get; }

        /// <summary>
        /// What's the active state we are in. 
        /// </summary>
        State ActiveState { get; set; }

        /// <summary>
        /// Transitioning to a new state. Will swap the states and fire the associated events.
        /// </summary>
        /// <param name="state">The new state to transition to.</param>
        protected void TransitionState(State state)
        {
            if (state == null)
            {
                Debug.Log("Transitioning to an empty state");
            }
            else if (MyStates.Contains(state) == false)
            {
                Debug.LogError("Transitioning to state that doesn't exist");
                return;
            }

            ActiveState?.OnExitState();

            ActiveState = state;

            ActiveState?.OnEnterState();
        }

        /// <summary>
        /// Transitioning to a new state. Will swap the states and fire the associated events.
        /// </summary>
        /// <param name="stateName">The new state name to transition to.</param>
        protected void TransitionState(string stateName)
        {
            State stateToTransitionTo = MyStates.FirstOrDefault(x => x.Name == stateName);
            TransitionState(stateToTransitionTo);
        }
    }

    [System.Serializable]
    public abstract class State
    {
        /// <summary>
        /// The name of the state to transition to. 
        /// </summary>
        public virtual string Name { get; }

        /// <summary>
        /// What happens when you enter this state.
        /// </summary>
        public abstract void OnEnterState();

        /// <summary>
        /// What happens when you exit this state.
        /// </summary>
        public abstract void OnExitState();

        /// <summary>
        /// What happens while you are in this state.
        /// </summary>
        public abstract void OnState();
    }
}