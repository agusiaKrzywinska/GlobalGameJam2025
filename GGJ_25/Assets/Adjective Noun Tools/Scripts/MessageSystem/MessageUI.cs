using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ANT
{
    public class MessageUI : Singleton<MessageUI>
    {
        [SerializeField, Tooltip("The text object the message copy will show up in.")]
        private TextMeshProUGUI textBox;
        [SerializeField, Tooltip("The sound effect player that will play the audio from.")]
        private SFXPlayer player;

        [SerializeField, Tooltip("The delay after the message finishes playing audio before it ends")]
        private float turnOffAfterSeconds = 0.5f;

        /// <summary>
        /// time left before hiding the message UI.
        /// </summary>
        private float currentTime = 0f;
        /// <summary>
        /// this action fires when the UI turns on or off. 
        /// </summary>
        public UnityAction OnMessageSetActive;
        /// <summary>
        /// Checks to see if a message is currently showing. 
        /// </summary>
        public bool ShowingMessage => gameObject.activeSelf;
        /// <summary>
        /// The current message that is showing. 
        /// </summary>
        private IMessage currentMessage;
        /// <summary>
        /// A queue of all the messages that have been told to show but haven't popped up yet. 
        /// </summary>
        private Queue<IMessage> messagesQueued = new Queue<IMessage>();

        /// <summary>
        /// all the 1 time played messages that have already played. 
        /// </summary>
        private List<IMessage> allPlayedOnceMessage = new List<IMessage>();

        /// <summary>
        /// Checks to see if a message has played only used for 1 time played messages.
        /// </summary>
        /// <param name="message">The message we want to check that has been played.</param>
        /// <returns>if the message has already been played.</returns>
        public bool HasPlayedMessage(IMessage message)
        {
            return allPlayedOnceMessage.Contains(message);
        }

        /// <summary>
        /// This will add the message to being 1 time played. 
        /// </summary>
        /// <param name="message">The message you want to add.</param>
        public void AddPlayedMessage(IMessage message)
        {
            if (HasPlayedMessage(message)) return;

            allPlayedOnceMessage.Add(message);
        }

        /// <summary>
        /// Add a new message to play. if there is nothing playing play this message that was just added. 
        /// </summary>
        /// <param name="messageToShow">The message you want to play.</param>
        public void QueueMessage(IMessage messageToShow)
        {
            messagesQueued.Enqueue(messageToShow);

            if (gameObject.activeSelf == false)
            {
                ShowNextMessageInQueue();
            }
        }
        /// <summary>
        /// start queuing the next message if there isn't any currently playing.
        /// </summary>
        public void ShowNextMessageInQueue()
        {
            //if there aren't new messages to show then stop.
            if (messagesQueued.Count == 0)
            {
                HideMessageUI();
                return;
            }

            //get the next message in the queue.
            IMessage messageToShow = messagesQueued.Dequeue();
            gameObject.SetActive(true);
            OnMessageSetActive.Invoke();
            //Update Audio and UI. 
            textBox.text = messageToShow.Text;
            player.AudioSource.reverbZoneMix = messageToShow.ReverbAmount;
            player.PlaySound(messageToShow.Audio);

            currentMessage = messageToShow;
        }
        /// <summary>
        /// Hide the message UI and reset times.
        /// </summary>
        public void HideMessageUI()
        {
            currentTime = 0;
            gameObject.SetActive(false);
            messagesQueued.Clear();
            OnMessageSetActive.Invoke();
        }

        private void Update()
        {
            // if there is no audio playing. 
            if (player.IsPlaying == false)
            {
                //increase the timer before the message ends.
                currentTime += Time.deltaTime;
                //if the timer is completed the trigger the on complete and show the next message. 
                if (currentTime >= turnOffAfterSeconds)
                {
                    currentMessage.OnComplete?.Invoke();
                    ShowNextMessageInQueue();
                }
            }
        }
    }
}