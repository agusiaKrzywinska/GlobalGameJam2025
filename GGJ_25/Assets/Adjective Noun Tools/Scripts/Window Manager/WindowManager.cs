using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ANT
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(SFXPlayer))]
    public class WindowManager : Singleton<WindowManager>
    {
        private Stack<IWindow> currentOpenWindows = new();
        /// <summary>
        /// a stack of all the current opened windows. 
        /// </summary>
        private static Stack<IWindow> CurrentOpenWindows => Instance.currentOpenWindows;
        /// <summary>
        /// All the names and windows that exist. 
        /// </summary>
        private Dictionary<string, IWindow> allWindows = new();
        private SFXPlayer player;
        /// <summary>
        /// The sound effect player that all the window sounds play from. 
        /// </summary>
        public static SFXPlayer Player => Instance.player;
        
        /// <summary>
        /// a bool to check if you should clear the audio queue. 
        /// </summary>
        private bool clearQueue = false;

        public static bool ClearQueue => Instance.clearQueue;
        /// <summary>
        /// a bool to see if you should play the idle message. 
        /// </summary>
        private static bool CanPlayIdleMessage = true;

        [SerializeField, Tooltip("The messages that play if you idle too long to know how to interact with the menu.")]
        private Clip[] reminderIdleMessage;
        [SerializeField, Tooltip("The sound effect that plays when you select a button.")]
        private Clip uiSelectionSFX;
        public static Clip UiSelectionSFX => Instance.uiSelectionSFX;
        [SerializeField, Tooltip("how much time of inactivity before the idle message plays.")]
        private float idleTimeBeforeMessagePlay = 3f;
        /// <summary>
        /// the current time what the player has been idling for. 
        /// </summary>
        private float currentIdlingTime = 0f;
        /// <summary>
        /// if the idle message has already played. 
        /// </summary>
        private bool idleMessageHasPlayed = false;

        protected override void OnSetup()
        {
            base.OnSetup();

            player = GetComponent<SFXPlayer>();
        }

        /// <summary>
        /// Adds a new window to the list of all windows. 
        /// </summary>
        /// <param name="window">The window you wish to add.</param>
        public static void RegisterWindow(IWindow window)
        {
            if (window != null)
                Instance.allWindows.Add(window.ScreenName, window);
        }

        /// <summary>
        /// Activates a window to be shown.
        /// </summary>
        /// <param name="window">The name of the window to display.</param>
        public static void ShowWindow(string window)
        {
            Instance.allWindows[window].Show();
        }

        /// <summary>
        /// Setups a window to show up. 
        /// </summary>
        /// <param name="window">The window to show up.</param>
        public static void ShowWindow(IWindow window)
        {
            // if there is a window already showing then hide it unless it's me. 
            if (CurrentOpenWindows.Count > 0 && CurrentOpenWindows.Peek() != window)
                CurrentOpenWindows.Peek().Hide();
            
            // if the window doesn't contain this one then add it. 
            if (!CurrentOpenWindows.Contains(window))
                CurrentOpenWindows.Push(window);

            // fire it's show event. 
            window.OnShow.Invoke();

            // turn off queueing sounds then select the button to have all their tracks cleared and then set it back to clear sounds. 
            Instance.clearQueue = false;
            Instance.DelayedExecuteRealTime(0.01f, () =>
            {
                EventSystem.current.SetSelectedGameObject(window.GetSelected());
                Instance.clearQueue = true;
            });
        }

        /// <summary>
        /// Will close a window. 
        /// </summary>
        /// <param name="window">The window to close</param>
        public static void CloseWindow(IWindow window)
        {
            // clear any currently selected items.
            window.CurrentSelected = null;
            // set this window to close. 
            window.OnClose.Invoke();
            // remove the window for the list. 
            CurrentOpenWindows.Pop();
            // if there are windows hidden underneath it then show it. 
            if (CurrentOpenWindows.Count > 0)
                CurrentOpenWindows.Peek().Show();
        }

        /// <summary>
        /// Generic way to close the currently previewing window. 
        /// </summary>
        private void CloseWindow()
        {
            CloseWindow(CurrentOpenWindows.Peek());
        }


        private void Update()
        {
            // if there aren't any open windows than just don't do anything. 
            if (CurrentOpenWindows.Count == 0)
            {
                currentIdlingTime = 0f;
                return;
            }

            //checking for idling 
            if (CanPlayIdleMessage)
            {
                // check for input. 
                if (Input.anyKey == false && player.AudioSource.isPlaying == false && idleMessageHasPlayed == false)
                {
                    currentIdlingTime += Time.deltaTime;
                    if (currentIdlingTime >= idleTimeBeforeMessagePlay)
                    {
                        //queue all idle audio
                        for (int i = 0; i < reminderIdleMessage.Length; i++)
                        {
                            player.PlaySound(reminderIdleMessage[i]);
                        }
                        //add current button selected
                        ButtonHighlighterUI selected = EventSystem.current?.currentSelectedGameObject?.GetComponent<ButtonHighlighterUI>();
                        if (selected != null)
                        {
                            selected.PlayButtonMessage();
                        }
                        // clean up values so that idle message doesn't play again. 
                        currentIdlingTime = 0f;
                        idleMessageHasPlayed = true;
                        CanPlayIdleMessage = false;
                    }
                }
                else
                {
                    //reset the timer since there as been interaction. 
                    currentIdlingTime = 0f;
                }

                // only play the idle message after there is new input. 
                if (idleMessageHasPlayed && Input.anyKey)
                {
                    idleMessageHasPlayed = false;
                }
            }

            // if you press cancel close the current opened window if it can be. 
            if (Input.GetButtonDown("Cancel"))
            {
                if (CurrentOpenWindows.Peek().CanBeClosedOnUpdate)
                    CurrentOpenWindows.Peek().Close();
            }

            // if nothing is selected reselect the current selected item on the active window. 
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(CurrentOpenWindows.Peek().CurrentSelected);
            }
            // if the current selected item doesn't match what is currently selected in the event system then swap it to the current selected one. 
            if (CurrentOpenWindows.Peek().CurrentSelected != EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject != null)
            {
                CurrentOpenWindows.Peek().CurrentSelected = EventSystem.current.currentSelectedGameObject;
            }
        }
    }
}