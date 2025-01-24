using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    // ? inherit popup manager. 
    // ? add any missing UIs to the manager
    // ? Update ShowNextPopup to call the UIs. 
    // ? add 'new' hiding property for inheriting Instance so it's type new type.  
    public abstract class PopupManager : Singleton<PopupManager>
    {
        /// <summary>
        /// All the current queued popup elements waiting to be shown. 
        /// </summary>
        private Queue<IPopup> queuedPopupsItems = new();

        /// <summary>
        /// A static bool to keep track if 
        /// </summary>
        public static bool IsShowingPopup { get; set; } = false;

        /// <summary>
        /// Called to Add a new popup to show up. 
        /// </summary>
        /// <param name="data">The popup to show up.</param>
        public void QueueNewPopup(IPopup data) => queuedPopupsItems.Enqueue(data);
        /// <summary>
        /// Checks to see if the popup already exists. 
        /// </summary>
        /// <param name="data">The popup to confirm if it exists</param>
        /// <returns></returns>
        public bool ContainsPopupQueued(IPopup data) => queuedPopupsItems.Contains(data);

        // Update is called once per frame
        void Update()
        {
            UpdateQueuedPopups();
        }

        /// <summary>
        /// Update the queued popups to show the next ones. 
        /// </summary>
        private void UpdateQueuedPopups()
        {
            //if there aren't any more in the queue don't check anything else. 
            if (queuedPopupsItems.Count <= 0)
                return;


            //if there is still a popup showing then wait. 
            if (IsShowingPopup)
                return;

            //Show next popup. 
            ShowNextPopup(queuedPopupsItems.Dequeue());
        }

        /// <summary>
        /// Calls the proper UI to display the popup. 
        /// </summary>
        /// <param name="popup">The popup that will be displayed.</param>
        protected abstract void ShowNextPopup(IPopup popup);
        /*
            switch (popup)
            {
                default:
                    Debug.Log("No popup UI made for this type.");
                    break;
                    //TODO add case by case basis for each different UI needed for each type. 
            }
        */
    }
}