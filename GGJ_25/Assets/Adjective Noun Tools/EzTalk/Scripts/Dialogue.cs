using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.EzTalk
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Adjective Noun Tools/EzTalk/Dialogue")]
    public class Dialogue : ScriptableObject
    {
        public Speaker primarySpeaker;
        public Speaker secondarySpeaker;

        [HideInInspector, SerializeReference]
        public BaseNode currentNode;
        [SerializeField]
        private UnityEvent onStartDialogue, onEndDialogue;
        public UnityEvent OnEndDialogue => onEndDialogue;
        public UnityEvent onSpeakerSwitch;

        [HideInInspector]
        public int currentInput = -1;

        [HideInInspector]
        public Speaker lastSpeaker = null;

        public NodeSettings defaultSettings;
        [HideInInspector]
        public int startNode = 0;
        [SerializeReference, HideInInspector]
        private List<BaseNode> nodes = new List<BaseNode>();
        public List<BaseNode> Nodes => nodes;
        [SerializeReference, HideInInspector]
        private List<BaseConnection> connections = new List<BaseConnection>();
        public List<BaseConnection> Connections => connections;
        [SerializeField, HideInInspector]
        private List<EzTalkParameter> parameters = new List<EzTalkParameter>();
        public List<EzTalkParameter> Parameters => parameters;
        public void StartDialogue(Action endEvent = null)
        {
            EzTalkCanvas.Instance.dialogue = this;

            EzTalkCanvas.Instance.gameObject.SetActive(true);
            currentNode = nodes[startNode];

            EzTalkCanvas.Instance.ResetAllTextBoxes();

            lastSpeaker = null;
            EzTalkCanvas.Instance.StartCoroutine(PlayDialogue(endEvent));
        }

        public void StartDialogue()
        {
            StartDialogue(null);
        }
        public void Skip()
        {
            if (currentNode is not TextNode node)
                return;

            if (!node.settings)
                return;

            if (node.settings.IsPlacing)
            {
                EzTalkCanvas.Instance.StopCoroutine(node.textPlace);
                node.settings.IsPlacing = false;
                EzTalkCanvas.Instance.UpdateMessage(GetFinalMessage(node.message), node.useSecondarySlot, node.Slot);
            }
            else if (EzTalkCanvas.Instance.CheckSkipTypewriter(node.useSecondarySlot, node.Slot))
            {
                EzTalkCanvas.Instance.SkipTypewriter(node.useSecondarySlot, node.Slot);
                Skip();
            }
            else if (currentNode.waitForCondition && !currentNode.waitForCondition.GetValue())
            {
                return;
            }
            else
            {
                currentNode.skipWait = true;
            }
        }
        private IEnumerator PlayDialogue(Action endDialogue)
        {
            onStartDialogue.Invoke();
            EzTalkCanvas.Instance.gameObject.SetActive(true);

            while (currentNode != null)
            {
                EzTalkCanvas.Instance.onStartNode.Invoke();
                yield return EzTalkCanvas.Instance.StartCoroutine(currentNode.DisplayNode());
                if (currentInput == -1)
                {
                    currentNode = null;
                }
                else
                {
                    if (currentNode.connections.Count <= currentInput)
                        break;
                    int nextNode = connections[currentNode.connections[currentInput]].nextNode;
                    if (nextNode == -1)
                    {
                        break;
                    }
                    currentNode = nodes[nextNode];
                }

                EzTalkCanvas.Instance.onEndNode.Invoke();
                currentInput = -1;
            }

            EzTalkCanvas.Instance.dialogue = null;
            EzTalkCanvas.Instance.gameObject.SetActive(false);
            EzTalkCanvas.Player.Stop();
            onEndDialogue.Invoke();

            if (endDialogue != null)
                endDialogue.Invoke();

        }
        public EzTalkParameter GetParameter(string parameter)
        {
            foreach (EzTalkParameter para in parameters)
            {
                if (para.name == parameter)
                {
                    return para;
                }
            }
            return null;
        }
        public string GetMessageWithParameters(string message)
        {
            if (message.IsEmptyString()) return message;

            parameters.Sort(delegate (EzTalkParameter x, EzTalkParameter y)
            {
                if (x.name == null && y.name == null) return 0;
                else if (x.name == null) return -1;
                else if (y.name == null) return 1;
                else return y.name.Length - x.name.Length;
            });

            foreach (EzTalkParameter parameter in parameters)
            {
                if (!parameter.GetValue().IsEmptyString())
                    message = message.Replace("$" + parameter.name, parameter.GetValue());
            }

            return message;
        }
        public string GetFinalMessage(string message)
        {
            message = GetMessageWithParameters(message);
            message = message.Replace(" | ", " ");
            message = message.Trim(' ');
            return message;
        }
        public bool AddParameter(string id, EzTalkParameter parameter)
        {
            if (id.IsEmptyString())
            {
                return false;
            }

            parameter.name = id;
            parameters.Add(parameter);
            return true;
        }
        public bool RemoveParameter(string id)
        {
            EzTalkParameter parameter = GetParameter(id);
            if (parameter != null)
            {
                parameters.Remove(parameter);
                return true;
            }
            return false;
        }
        public void SetBool(string parameter, bool value)
        {
            EzTalkParameter para = GetParameter(parameter);
            if (para == null)
            {
                Debug.LogError($"Parameter {parameter} does not exist");
                return;
            }
            para.SetValue(value);
        }
        public void SetString(string parameter, string value)
        {
            EzTalkParameter para = GetParameter(parameter);
            if (para == null)
            {
                Debug.LogError($"Parameter {parameter} does not exist");
                return;
            }
            para.SetValue(value);
        }
        public void SetFloat(string parameter, float value)
        {
            EzTalkParameter para = GetParameter(parameter);
            if (para == null)
            {
                Debug.LogError($"Parameter {parameter} does not exist");
                return;
            }
            para.SetValue(value);
        }
        public void SetInt(string parameter, int value)
        {
            EzTalkParameter para = GetParameter(parameter);
            if (para == null)
            {
                Debug.LogError($"Parameter {parameter} does not exist");
                return;
            }
            para.SetValue(value);
        }
    }
}