using ANT.Audio;
using ANT.ScriptableProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ANT.EzTalk
{

    [System.Serializable]
    public class BaseNode
    {
        public string title;
        public List<int> connections;
        [HideInInspector]
        public bool isEndNode => connections.Count == 0;
        [HideInInspector]
        public Vector2 positionInEditor;
        public UnityEvent onStartNode;
        public UnityEvent onEndNode;
        public UnityEvent onWaitNode;
        public ScriptableBool waitForCondition;

        public bool waitForPlayerResponse;
        public bool skipWait = false;

        public Sound VO;

        public IEnumerator DisplayNode()
        {
            var dialogue = EzTalkCanvas.Instance.dialogue;
            skipWait = false;
            onStartNode.Invoke();

            yield return AfterStartNode();

            if (waitForCondition)
            {
                onWaitNode.Invoke();
                while (!waitForCondition.GetValue())
                {
                    yield return new WaitForEndOfFrame();
                }
            }


            yield return AfterWaitNode();

            if (isEndNode)
            {
                //do nothing else
            }
            else if (!waitForPlayerResponse)
            {
                GetNextConnection();
            }
            else
            {
                EzTalkCanvas.Instance.ToggleReplies(true);
                while (dialogue.currentInput == -1)
                {
                    yield return null;

                    if (EventSystem.current.currentSelectedGameObject == null)
                    {
                        EzTalkCanvas.Instance.SelectFirstReplyButton();
                    }
                }
                yield return new WaitUntil(() => dialogue.currentInput != -1);
            }

            EzTalkCanvas.Instance.ToggleReplies(false);
            onEndNode.Invoke();
        }

        public virtual IEnumerator AfterStartNode()
        {
            yield return null;
        }

        public virtual IEnumerator AfterWaitNode()
        {
            yield return null;
        }
        public void SetUpIfWaitForResponseNode(Dialogue dialogue)
        {
            waitForPlayerResponse = false;
            for (int i = 0; i < connections.Count; i++)
            {
                if (dialogue.Connections[connections[i]] is ResponseConnection responseConnection && !responseConnection.response.IsEmptyString())
                {
                    waitForPlayerResponse = true;
                }
            }
        }
        private void GetNextConnection()
        {
            var dialogue = EzTalkCanvas.Instance.dialogue;
            for (int i = 0; i < connections.Count; i++)
            {
                var connection = dialogue.Connections[connections[i]];
                if (connection.CanShow)
                {
                    dialogue.currentInput = i;
                    break;
                }
            }
        }
    }

    [System.Serializable]
    public class TextNode : BaseNode
    {
        public string message;
        public bool useSecondarySlot;
        public bool overrideSpeaker;
        public Speaker speakerOverride;
        public bool overrideStyle;
        public int newStyleToOverride;
        [HideInInspector]
        public NodeSettings currentSettings;
        public NodeSettings settings;
        public bool overrideSetting;
        public NodeSettings overriddenSettings;

        public int Slot => overrideStyle ? newStyleToOverride : 0;

        public Coroutine textPlace;

        public override IEnumerator AfterStartNode()
        {
            var dialogue = EzTalkCanvas.Instance.dialogue;
            currentSettings = overrideSetting ? overriddenSettings : settings;
            if (!currentSettings)
            {
                Debug.LogError($"Missing settings on {dialogue.name} at node {title}", dialogue);
            }

            if (!message.IsEmptyString() && currentSettings)
            {
                EzTalkCanvas.Instance.onStartPlacing.Invoke();
                var speaker = overrideSpeaker ? speakerOverride : useSecondarySlot ? dialogue.secondarySpeaker : dialogue.primarySpeaker;
                EzTalkCanvas.Instance.UpdateSpeakerUI(useSecondarySlot, Slot, speaker);
                if (dialogue.lastSpeaker != speaker)
                    dialogue.onSpeakerSwitch.Invoke();
                dialogue.lastSpeaker = speaker;

                if (VO != null)
                {
                    EzTalkCanvas.Player.PlaySound(VO);
                }

                textPlace = EzTalkCanvas.Instance.StartCoroutine(currentSettings.PlaceMessage(dialogue.GetFinalMessage(message), speaker, useSecondarySlot, Slot));
                while (currentSettings.IsPlacing)
                {
                    yield return new WaitForEndOfFrame();
                }
                if (!waitForPlayerResponse)
                {
                    EzTalkCanvas.Instance.onFinishedPlacing.Invoke();
                }
            }
        }

        public override IEnumerator AfterWaitNode()
        {
            var dialogue = EzTalkCanvas.Instance.dialogue;
            //skip wait on empty nodes
            if (!message.IsEmptyString() && currentSettings && !waitForPlayerResponse && currentSettings.TimeOnScreen > 0)
            {
                float currentTime = 0f;
                while (EzTalkCanvas.Player.AudioSource.isPlaying)
                {
                    if (skipWait)
                    {
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
                //waiting for time delay
                while (currentTime < currentSettings.TimeOnScreen || currentSettings.WaitForInputToContinue)
                {
                    if (skipWait)
                    {
                        skipWait = false;
                        break;
                    }
                    currentTime = Mathf.Clamp(currentTime + Time.deltaTime, 0, currentSettings.TimeOnScreen);
                    yield return new WaitForEndOfFrame();
                }

            }
        }
    }
}
