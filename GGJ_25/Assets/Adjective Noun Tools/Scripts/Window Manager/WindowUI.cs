using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ANT
{
    public class WindowUI : MonoBehaviour, IWindow
    {
        [SerializeField, Tooltip("if you press back on a controller does this screen close.")]
        private bool closeOnUpdate;

        public bool CanBeClosedOnUpdate => closeOnUpdate;
        [SerializeField, Tooltip("A screen name for the window.")]
        private string screenName;
        public string ScreenName => screenName;

        [SerializeField, Tooltip("ordered by first selected what buttons should be selected unless there is something currently selected or it's not selectable.")]
        private GameObject[] firstSelected;
        public GameObject[] FirstSelected { get => firstSelected; }

        private GameObject currentSelected;
        public GameObject CurrentSelected { get => currentSelected; set => currentSelected = value; }

        [SerializeField, Tooltip("Fired when the screen is closed.")]
        private UnityEvent onClose;
        public UnityEvent OnClose => onClose;

        [SerializeField, Tooltip("Fired when the screen is shown. ")]
        private UnityEvent onShow;
        public UnityEvent OnShow => onShow;

        [SerializeField, Tooltip("Fired when the screen is covered by another window. ")]
        private UnityEvent onHide;
        public UnityEvent OnHide => onHide;
        [SerializeField, Tooltip("Turn off when it's started up.")]
        private bool hideOnStart = true;

        [SerializeField, Tooltip("All the sound effects that play when the screen first shows up.")]
        private SoundDataHolder[] onLoadWindowSfx;

        private void Awake()
        {
            SetupWindow();
            if (hideOnStart)
            {
                gameObject.SetActive(false);
                return;
            }

            Show();
        }

        public void Show()
        {
            //clear any active sounds that are playing and queue this windows sound effects. 
            WindowManager.Player.ClearAudioQueue();
            for (int i = 0; i < onLoadWindowSfx.Length; i++)
            {
                WindowManager.Player.PlaySound(onLoadWindowSfx[i]);
            }
            WindowManager.ShowWindow(this);
        }

        public void Hide()
        {
            // when you hide save last selected element. 
            CurrentSelected = EventSystem.current.currentSelectedGameObject;
            OnHide.Invoke();
        }

        public void Close()
        {
            WindowManager.CloseWindow(this);
        }

        public void SetupWindow()
        {
            WindowManager.RegisterWindow(this);
        }

        /// <summary>
        /// called to set the event system to not select anything. 
        /// </summary>
        public void SelectNothing()
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}