using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace ANT
{
    // ? inherit from PopupUI to make each unique UI. 
    // ? add any UI references to make it display properly. 
    // ? update UpdatePopup to display the popup. 
    public abstract class PopupUI<T> : MonoBehaviour where T : IPopup
    {
        [Required, SerializeField, Tooltip("the parent gameobject that holds all the UI.")]
        private GameObject parent;

        /// <summary>
        /// This displays the popup and waits for the time to be completed before hiding it. 
        /// </summary>
        /// <param name="data">The popup data you wish to show</param>
        public void ShowPopup(T data)
        {
            if (data == null)
            {
                HidePopup();
                return;
            }

            PopupManager.IsShowingPopup = true;
            UpdatePopup(data);
            parent.SetActive(true);
            this.DelayedExecute(data.TimeOnScreen, HidePopup);
        }
        /// <summary>
        /// Actually updates the UIs to display the specific popup. 
        /// </summary>
        /// <param name="data">The data to show in the UI. </param>
        protected abstract void UpdatePopup(T data);

        /// <summary>
        /// Turns off the popup UI. 
        /// </summary>
        public void HidePopup()
        {
            parent.SetActive(false);

            PopupManager.IsShowingPopup = false;
        }


    }

    // ? inherit IPopup to allow for it to be popped up and set how long it will be on screen for. 
    public interface IPopup
    {
        /// <summary>
        /// The amount of time the popup will show up for. 
        /// </summary>
        float TimeOnScreen { get; }
    }
}
