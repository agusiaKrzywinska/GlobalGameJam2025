using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ANT.Audio;
using NaughtyAttributes;
using UnityEngine.EventSystems;

namespace ANT.EzTalk
{
    [AddComponentMenu("Adjective Noun Tools/EzTalk/EzTalk Canvas")]
    public class EzTalkCanvas : Singleton<EzTalkCanvas>
    {
        [SerializeField, Required]
        private EzTalkSpeakerUI[] primarySpeaker;
        [SerializeField]
        private EzTalkSpeakerUI[] secondarySpeaker;
        [SerializeField]
        private SFXPlayer player = null;
        public static SFXPlayer Player => (Instance.currentSpeakerUI && Instance.currentSpeakerUI.SFXPlayer) ? Instance.currentSpeakerUI.SFXPlayer : Instance.player;
        [SerializeField]
        protected List<Button> replies = new List<Button>();
        [SerializeField, ReadOnly]
        protected EzTalkText[] repliesText;

        public Dialogue dialogue;

        [SerializeField]
        protected UnityEvent onStartConversation, onEndConversation;
        public UnityEvent onStartPlacing, onFinishedPlacing, onStartNode, onEndNode;
        public UnityAction OnEztalkSetActive;

        [SerializeField]
        protected bool canSkipWithClick = true;

        private EzTalkSpeakerUI currentSpeakerUI;

        private void Start()
        {

        }

        private void OnEnable()
        {
            if (dialogue == null)
                return;

            
            OnEztalkSetActive.Invoke();

            onStartConversation.Invoke();

            TurnOnStyle(false, 0);

            primarySpeaker[0].UpdateSpeakerUI(dialogue.primarySpeaker);
            if (secondarySpeaker.Exists(0))
                secondarySpeaker[0].UpdateSpeakerUI(dialogue.secondarySpeaker);

            ToggleReplies(false);
        }

        private void TurnOnStyle(bool isSecondarySpeaker, int slot)
        {
            for (int i = 0; i < primarySpeaker.Length; i++)
            {
                primarySpeaker[i].SetUIState(false);
            }
            primarySpeaker[isSecondarySpeaker ? 0 : slot].SetUIState(true);

            for (int i = 0; i < secondarySpeaker.Length; i++)
            {
                secondarySpeaker[i].SetUIState(false);
            }
            secondarySpeaker[isSecondarySpeaker ? slot : 0].SetUIState(true);
        }

        private void OnDisable()
        {
            onEndConversation.Invoke();
        }

        protected override void OnSetup()
        {
            gameObject.SetActive(false);
            if (player == null)
                player = GetComponent<SFXPlayer>();
            if (player == null)
            {
                Debug.Log("No SFX Player on EzTalkCanvas");
            }
            SetUpReplyButtons();
        }

        private void SetUpReplyButtons()
        {
            repliesText = new EzTalkText[replies.Count];
            for (int i = 0; i < replies.Count; i++)
            {
                SetupButton(i);
            }
        }

        protected virtual void SetupButton(int position)
        {
            void TriggerConnection(int position)
            {
                dialogue.Connections[dialogue.currentNode.connections[position]].TriggerConnection();
            }

            replies[position].onClick.AddListener(() => dialogue.currentInput = position);
            replies[position].onClick.AddListener(() => TriggerConnection(position));
            repliesText[position] = new EzTalkText();
            repliesText[position].SetupTextReferences(replies[position].transform);
            replies[position].gameObject.SetActive(false);

        }

        public void ResetAllTextBoxes()
        {
            for (int i = 0; i < primarySpeaker.Length; i++)
            {
                UpdateMessage("", false, i);
            }

            for (int i = 0; i < secondarySpeaker.Length; i++)
            {
                UpdateMessage("", true, i);
            }
        }


        public void Skip()
        {
            dialogue.Skip();
        }

        public void SkipAll()
        {
            if (dialogue == null) return;

            dialogue.currentNode = null;
            dialogue.OnEndDialogue.Invoke();
            dialogue = null;
            player.Stop();
            gameObject.SetActive(false);
        }

        public void UpdateMessage(string message, bool useSecondarySlot, int slot)
        {
            if (useSecondarySlot && secondarySpeaker.Exists(slot))
                secondarySpeaker[slot].UpdateMessage(message);
            else
                primarySpeaker[slot].UpdateMessage(message);
        }

        public bool CheckSkipTypewriter(bool useSecondarySlot, int slot)
        {
            if (useSecondarySlot && secondarySpeaker.Exists(slot))
                return secondarySpeaker[slot].HasTypewriterToSkip();
            else
                return primarySpeaker[slot].HasTypewriterToSkip();
        }

        public void SkipTypewriter(bool useSecondarySlot, int slot)
        {
            if (useSecondarySlot && secondarySpeaker.Exists(slot))
                secondarySpeaker[slot].SkipTypeWriter();
            else
                primarySpeaker[slot].SkipTypeWriter();
        }

        public void UpdateSpeakerUI(bool useSecondarySlot, int slot, Speaker speaker)
        {
            TurnOnStyle(useSecondarySlot, slot);

            primarySpeaker[useSecondarySlot ? 0 : slot].UpdateUI(!useSecondarySlot);
            primarySpeaker[useSecondarySlot ? 0 : slot].UpdateSpeakerUI(speaker);
            if (secondarySpeaker[useSecondarySlot ? slot : 0])
            {
                secondarySpeaker[useSecondarySlot ? slot : 0].UpdateUI(useSecondarySlot);
                secondarySpeaker[useSecondarySlot ? slot : 0].UpdateSpeakerUI(speaker);
            }

            currentSpeakerUI = useSecondarySlot ? secondarySpeaker[slot] : primarySpeaker[slot];
        }

        public virtual void ToggleReplies(bool state)
        {
            if (state)
                ShowReplies();
            else
                HideReplies();
        }

        private void ShowReplies()
        {
            var node = dialogue.currentNode;
            for (int i = 0; i < replies.Count; i++)
            {
                if (node.connections.Count <= i)
                    ShowReply(i, false);
                else
                    ShowReply(i, dialogue.Connections[node.connections[i]].ShowReply);
            }

            SelectFirstReplyButton();
        }

        public void StopAllAudio()
        {
            player.Stop();
            for (int i = 0; i < primarySpeaker.Length; i++)
            {
                primarySpeaker[i].SFXPlayer.Stop();
            }

            for (int i = 0; i < secondarySpeaker.Length; i++)
            {
                primarySpeaker[i].SFXPlayer.Stop();
            }
        }

        public void SelectFirstReplyButton()
        {
            /*
            if (EventSystem.current.alreadySelecting == false)
                EventSystem.current.SetSelectedGameObject(replies[0].gameObject);
            */
        }

        public void SelectReply(int id)
        {
            if (dialogue.currentNode.isEndNode || dialogue.currentNode.waitForPlayerResponse == false)
                return;

            //force to skip if buttons aren't showing
            if (replies[id].gameObject.activeInHierarchy == false)
            {
                Skip();
                Skip();
            }
            this.DelayedExecute(0.01f,  replies[id].onClick.Invoke);
            //EventSystem.current.SetSelectedGameObject(replies[id].gameObject);
        }

        protected virtual void ShowReply(int position, bool state)
        {
            replies[position].gameObject.SetActive(state);
            if (state == false)
            {
                repliesText[position].Write("Whatever");
                return;
            }

            var connection = dialogue.Connections[dialogue.currentNode.connections[position]] as ResponseConnection;
            repliesText[position].Write(dialogue.GetFinalMessage(connection.response));
        }

        private void HideReplies()
        {
            for (int i = 0; i < replies.Count; i++)
            {
                replies[i].gameObject.SetActive(false);
            }
        }
    }
}