using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ANT.Audio;

namespace ANT.EzTalk
{
    [CreateAssetMenu(fileName = "New Settings", menuName = "Adjective Noun Tools/EzTalk/Node Settings")]
    public class NodeSettings : ScriptableObject
    {
        [SerializeField]
        private Placer placeStyle;
        [SerializeField]
        private float waitBetweenEachPlace;
        [SerializeField, Tooltip("This event is called each each time the text updates.")]
        private UnityEvent onPlaceText;
        [SerializeField]
        private float timeOnScreen;
        public float TimeOnScreen => timeOnScreen;

        [SerializeField, Header("Audio Settings")]
        private bool doSounds = false;
        [SerializeField]
        private bool placeWithScaledTime = true;
        public bool IsPlacing { get; set; }
        [SerializeField]
        private bool waitForInputToContinue = false;
        public bool WaitForInputToContinue => waitForInputToContinue;
        public IEnumerator PlaceMessage(string message, Speaker speaker, bool useSecondarySlot, int slot)
        {
            IsPlacing = true;
            List<string> chunks = GetChunks(message, GetStyles(message));

            //place message
            for (int i = 0; i < chunks.Count; i++)
            {
                EzTalkCanvas.Instance.UpdateMessage(chunks[i], useSecondarySlot, slot);
                onPlaceText.Invoke();
                if (doSounds)
                    speaker.PlayVoice();
                yield return placeWithScaledTime ? new WaitForSeconds(waitBetweenEachPlace) : new WaitForSecondsRealtime(waitBetweenEachPlace);
            }
            IsPlacing = false;
        }
        private string AddRichText(string message, int position, List<(string style, int startPos, int messagePos)> styles)
        {
            string newMessage = message;
            int newLength = 0;
            string afterMessage = "";
            for (int i = 0; i < styles.Count; i++)
            {
                if (position >= styles[i].startPos)
                {
                    newMessage = newMessage.Insert(styles[i].messagePos + newLength, styles[i].style);
                    newLength += styles[i].style.Length;
                }
                else
                {
                    afterMessage += styles[i].style;
                }
            }
            return newMessage + afterMessage;
        }
        private bool IsInStyleHeader(int position, List<(string style, int startPos, int messagePos)> styles)
        {
            for (int i = 0; i < styles.Count; i++)
            {
                if (position >= styles[i].startPos && position < styles[i].startPos + styles[i].style.Length)
                    return true;
            }
            return false;
        }
        private List<(string style, int startPos, int messagePos)> GetStyles(string message)
        {
            var styles = new List<(string style, int startPos, int messagePos)>();

            int actualMessageLength = 0;
            for (int i = 0; i < message.Length; i++)
            {
                if (message[i] == '<')
                {
                    for (int j = i; j < message.Length; j++)
                    {
                        if (message[j] == '>')
                        {
                            styles.Add((message.Substring(i, j - i + 1), i, actualMessageLength));
                            i = j;
                            break;
                        }
                    }
                }
                else
                {
                    actualMessageLength++;
                }
            }

            return styles;
        }
        private List<string> GetChunks(string message, List<(string style, int startPos, int messagePos)> styles)
        {
            List<string> chunks = new List<string>();
            string currentMessage = "";
            //parse message into the message chunks
            switch (placeStyle)
            {
                case Placer.Letter:
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (IsInStyleHeader(i, styles)) continue;
                        currentMessage += message[i];
                        chunks.Add(AddRichText(currentMessage, i, styles));
                    }
                    break;
                case Placer.Word:
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (IsInStyleHeader(i, styles)) continue;
                        currentMessage += message[i];
                        if (message[i] == ' ' || i == message.Length - 1)
                            chunks.Add(AddRichText(currentMessage, i, styles));
                    }
                    break;
                case Placer.Sentence:
                    for (int i = 0; i < message.Length; i++)
                    {
                        if (IsInStyleHeader(i, styles)) continue;
                        currentMessage += message[i];
                        if (message[i] == '.' || message[i] == '\n' || i == message.Length - 1)
                            chunks.Add(AddRichText(currentMessage, i, styles));
                    }
                    break;
                case Placer.All:
                    chunks.Add(message);
                    break;
            }

            return chunks;
        }
        public NodeSettings Clone()
        {
            NodeSettings instance = ScriptableObject.CreateInstance<NodeSettings>();
            instance.placeStyle = placeStyle;
            instance.waitBetweenEachPlace = waitBetweenEachPlace;
            instance.timeOnScreen = timeOnScreen;
            instance.onPlaceText = onPlaceText;
            return instance;
        }
    }
}