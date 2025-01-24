using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    public class AnimatorHelper : MonoBehaviour
    {
        [SerializeField, Tooltip("The animator component which you want to play these animations from")]
        private Animator animator = null;
        [SerializeField, NaughtyAttributes.AnimatorParam(nameof(animator)), Tooltip("The parameter that you want to use with set bool and set float methods")]
        private string parameter = "";

        /// <summary>
        /// Allows for a user to use the Set Bool method on an animator through events. 
        /// </summary>
        /// <param name="value">The new value for the value of the bool parameter. </param>
        public void SetBool(bool value)
        {
            animator.SetBool(parameter, value);
        }

        /// <summary>
        /// Allows for a user to use the Set Float method on an animator through events.  
        /// </summary>
        /// <param name="value">The new value for the value of the float parameter. </param>
        public void SetFloat(float value)
        {
            animator.SetFloat(parameter, value);
        }
    }
}
