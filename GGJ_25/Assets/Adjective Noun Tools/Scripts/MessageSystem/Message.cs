using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.Audio;
using UnityEngine.Events;

namespace ANT
{
    [CreateAssetMenu(fileName = "Message", menuName = "ANT/Message")]
    public class Message : ScriptableObject, IMessage
    {
        [SerializeField, TextArea, Tooltip("The message text that will show up.")]
        private string text;
        public string Text => text;
        [SerializeField, Tooltip("The audio clip that makes up the message.")]
        private SoundDataHolder audio;
        public SoundDataHolder[] Audio => new SoundDataHolder[] { audio };

        UnityAction IMessage.OnComplete { get => onCompleteAction; }

        private UnityAction onCompleteAction;

        [SerializeField, Tooltip("The unity event that will fire when the message completes.")]
        private UnityEvent onComplete;

        [SerializeField, Tooltip("What should be the reverb for the audio.")]
        private float reverbAmount;
        public float ReverbAmount => reverbAmount;


        private void Awake()
        {
            onCompleteAction += onComplete.Invoke;
        }

        private void OnDestroy()
        {
            onCompleteAction -= onComplete.Invoke;
        }

        /// <summary>
        /// Queue the message to show up.
        /// </summary>
        public void ShowMessage()
        {
            MessageUI.Instance.QueueMessage(this);
        }

        /// <summary>
        /// Queue the message to only play once ever. 
        /// </summary>
        public void ShowMessageOneTime()
        {
            if (MessageUI.Instance.HasPlayedMessage(this) == false)
            {
                ShowMessage();
                MessageUI.Instance.AddPlayedMessage(this);
            }
        }

    }
}