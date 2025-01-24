using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;

namespace ANT
{
    public interface IWindow
    {
        /// <summary>
        /// A screen name for the window. 
        /// </summary>
        string ScreenName { get; }
        /// <summary>
        /// if you press back on a controller does this screen close. 
        /// </summary>
        bool CanBeClosedOnUpdate { get; }
        /// <summary>
        /// Fired when the screen is shown. 
        /// </summary>
        UnityEvent OnShow { get; }
        /// <summary>
        /// Fired when the screen is closed. 
        /// </summary>
        UnityEvent OnClose { get; }
        /// <summary>
        /// Fired when the screen is covered by another window. 
        /// </summary>
        UnityEvent OnHide { get; }
        /// <summary>
        /// What should be selected on this for the window by default. 
        /// </summary>
        /// <returns>The default selected element.</returns>
        GameObject GetSelected()
        {
            return CurrentSelected ?? FirstSelected.FirstOrDefault(x => x.activeSelf
            && ((x.GetComponent<Button>() != null && x.GetComponent<Button>().interactable) || x.GetComponent<Button>() == null));
        }
        /// <summary>
        /// ordered by first selected what buttons should be selected unless there is something currently selected or it's not selectable. 
        /// </summary>
        GameObject[] FirstSelected { get; }
        /// <summary>
        /// storing a current selected element in case it was hidden and reselected. 
        /// </summary>
        GameObject CurrentSelected { get; set; }

        /// <summary>
        /// Called to show the window.
        /// </summary>
        void Show();

        /// <summary>
        /// called to hide the window. Which means another window is covering this one. 
        /// </summary>
        void Hide();

        /// <summary>
        /// called to close the window. 
        /// </summary>
        void Close();

        /// <summary>
        /// Called for setup any specifics for the window on start. 
        /// </summary>
        void SetupWindow();
    }
}