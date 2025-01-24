using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using UnityEngine.Events;

namespace ANT
{
    public class MessageObject : IMessage
    {
        private string text;
        private SoundDataHolder[] audio;
        public string Text => text;
        public SoundDataHolder[] Audio => audio;
        private float reverbAmount;
        public float ReverbAmount => reverbAmount;

        UnityAction IMessage.OnComplete { get => onComplete; }

        public UnityAction onComplete;

        /// <summary>
        /// Setup the object with all the values needed. 
        /// </summary>
        /// <param name="text">the message text copy.</param>
        /// <param name="audio">the message audio clips. </param>
        /// <param name="reverb">the change in reverb for the audio source.</param>
        /// <param name="onComplete">fires after the message is complete.</param>
        public MessageObject(string text, SoundDataHolder[] audio, float reverb, UnityAction onComplete)
        {
            this.text = text;
            this.audio = audio;
            this.reverbAmount = reverb;
            this.onComplete = onComplete;
        }

        /// <summary>
        /// Add this message to the queue so it shows up. 
        /// </summary>
        public void ShowMessage()
        {
            MessageUI.Instance.QueueMessage(this);
        }
    }

    [System.Serializable]
    public class TextAudio
    {
        public string Text;
        public SoundDataHolder Audio;
    }
}