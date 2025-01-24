using System;
using UnityEngine;
using UnityEditor;
using ANT.ScriptableProperties;
using System.Collections.Generic;

namespace ANT.EzTalk
{
    public class NodeEditor
    {
        public Rect rect;
        public Rect displayInfoRect;
        public Rect clickZone;
        public bool isDragged;
        public bool isExpanded;
        private readonly float borderPercent = 0.1f;
        private float borderWidth;
        private float heightOfNodeInfo = 0;

        private GUIStyle multiline;

        public BaseNode nodeData;

        public int nodePosition;

        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;

        public GUIStyle style;

        public Action<NodeEditor> OnRemoveNode;

        public Color GetColor()
        {
            Color color = new Color();
            //base node color
            color = color.GetHexColor("#9a490a");
            if (nodeData is TextNode textData)
            {
                color = color.GetHexColor("#0a5b9a");
                if (textData.useSecondarySlot || textData.overrideSpeaker)
                {
                    color = color.GetHexColor("#139a0a");
                }
            }
            return color;
        }

        public NodeEditor(Vector2 position, float width, float height, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<NodeEditor> OnClickRemoveNode, BaseNode data)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = EzTalkEditor.instance.nodeStyle;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, EzTalkEditor.instance.inPointStyle, OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, EzTalkEditor.instance.outPointStyle, OnClickOutPoint);
            OnRemoveNode = OnClickRemoveNode;

            borderWidth = width * borderPercent / 2;
            heightOfNodeInfo = height * 2;
            displayInfoRect = new Rect(position.x + borderWidth, position.y + height, width - borderWidth * 2, heightOfNodeInfo);

            this.nodeData = data;
            this.nodeData.positionInEditor = position;

            this.nodeData.connections = new List<int>();

        }

        public bool IsOnScreen(Rect screen, float zoom)
        {
            if ((rect.x + rect.width) * zoom < 0)
                return false;
            else if (screen.width < rect.x * zoom)
                return false;
            if ((rect.y + rect.height) * zoom < 0)
                return false;
            else if (screen.height < rect.y * zoom)
                return false;
            return true;
        }

        public void Drag(Vector2 delta, float zoom)
        {
            rect.position += delta / zoom;
            displayInfoRect.position += delta / zoom;
            nodeData.positionInEditor = rect.position;
        }

        public void DrawNodeSidebarInfo()
        {
            string label;
            float oldWidth = EditorGUIUtility.labelWidth;

            multiline = new GUIStyle(GUI.skin.label);
            multiline.wordWrap = true;
            multiline.richText = true;

            label = "Title";
            SetLabelSize(label);

            //contents
            nodeData.title = EditorGUILayout.TextField(label, nodeData.title);
            EditorStyles.textField.wordWrap = true;
            var textData = nodeData as TextNode;
            SerializedObject serializedObject = new SerializedObject(EzTalkEditor.instance.dialogue);

            var node = serializedObject.FindProperty("nodes").GetArrayElementAtIndex(nodePosition);

            if (textData == null)
            {
                EditorGUILayout.HelpBox("This node is empty and will just continue to the next connection.", MessageType.Info);
            }
            else
            {
                textData.message = EditorGUILayout.TextArea(textData.message);

                GUILayout.Label(EzTalkEditor.instance.dialogue.GetFinalMessage(textData.message), multiline);
                EditorGUILayout.Space(5f);

                //speaker settings
                label = "Use Secondary Speaker";
                SetLabelSize(label);
                textData.useSecondarySlot = EditorGUILayout.Toggle(label, textData.useSecondarySlot);
                label = "Override Speaker";
                SetLabelSize(label);
                textData.overrideSpeaker = EditorGUILayout.Toggle(label, textData.overrideSpeaker);
                if (textData.overrideSpeaker)
                {
                    textData.speakerOverride = (Speaker)EditorGUILayout.ObjectField(label, textData.speakerOverride, typeof(Speaker), false);
                }

                label = "Use Different Style";
                SetLabelSize(label);
                textData.overrideStyle = EditorGUILayout.Toggle(label, textData.overrideStyle);
                label = "Override Style";
                SetLabelSize(label);
                if (textData.overrideStyle)
                {
                    textData.newStyleToOverride = EditorGUILayout.IntField(label, textData.newStyleToOverride);
                }

                label = "Settings";
                SetLabelSize(label);
                textData.settings = (NodeSettings)EditorGUILayout.ObjectField(label, textData.settings, typeof(NodeSettings), false);
                label = "Override Settings";
                SetLabelSize(label);
                textData.overrideSetting = EditorGUILayout.Toggle(label, textData.overrideSetting);
                if (textData.overrideSetting)
                {
                    if (textData.overriddenSettings == null && textData.settings != null)
                    {
                        textData.overriddenSettings = textData.settings.Clone();
                    }
                    //add draw the overridden settings to the node info
                    EditorGUILayout.PropertyField(node.FindPropertyRelative("overridenSettings"));
                }
            }
            label = "Voice Line Audio";
            SetLabelSize(label);
            nodeData.VO = (Audio.Sound)EditorGUILayout.ObjectField(label, nodeData.VO, typeof(Audio.Sound), false);

            EditorGUILayout.PropertyField(node.FindPropertyRelative("onStartNode"));
            EditorGUILayout.PropertyField(node.FindPropertyRelative("onEndNode"));
            if (nodeData.waitForCondition)
            {
                EditorGUILayout.PropertyField(node.FindPropertyRelative("onWaitNode"));
            }
            serializedObject.ApplyModifiedProperties();

            label = "Wait for condition";
            SetLabelSize(label);
            nodeData.waitForCondition = (ScriptableBool)EditorGUILayout.ObjectField(label, nodeData.waitForCondition, typeof(ScriptableBool), false);

            EzTalkStyles.DrawUILine(new Color(0, 0, 0, 0.5f));

            //drawing the connections
            //going over all the connections and drawing them 
            for (int i = 0; i < nodeData.connections.Count; i++)
            {
                if (nodeData.connections[i] >= EzTalkEditor.instance.dialogue.Connections.Count)
                {
                    Debug.LogError("Connection does not exist");
                    nodeData.connections.RemoveAt(i);
                    continue;
                }

                BaseConnection myConnection = EzTalkEditor.instance.dialogue.Connections[nodeData.connections[i]];
                var connection = serializedObject.FindProperty("connections").GetArrayElementAtIndex(nodeData.connections[i]);
                GUILayout.Label(myConnection.title, multiline);
                //Setting up the height of the connection point. 
                if (myConnection is ResponseConnection responseConnection)
                {
                    responseConnection.response = EditorGUILayout.TextArea(responseConnection.response);
                    if (!responseConnection.response.IsEmptyString())
                    {
                        GUILayout.Label(EzTalkEditor.instance.dialogue.GetFinalMessage(responseConnection.response), multiline);
                    }
                }

                label = "Condition";
                SetLabelSize(label);
                EditorGUILayout.BeginHorizontal();
                myConnection.condition = (ScriptableBool)EditorGUILayout.ObjectField(label, myConnection.condition, typeof(ScriptableBool), false);
                if (myConnection.condition == null && GUILayout.Button("New"))
                {
                    myConnection.condition = ScriptableObject.CreateInstance<ScriptableBool>();
                    AssetDatabase.CreateAsset(myConnection.condition, $"Assets/{myConnection.title}.asset");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.PropertyField(connection.FindPropertyRelative("onTrigger"));
                serializedObject.ApplyModifiedProperties();
            }

            EditorGUIUtility.labelWidth = oldWidth;
        }

        public void Draw(int position, float zoom)
        {
            this.nodePosition = position;

            inPoint.Draw(zoom);
            outPoint.Draw(zoom);


            var pastColor = GUI.backgroundColor;
            var speakInfo = "";
            GUI.backgroundColor = GetColor();

            var textData = nodeData as TextNode;
            if (textData != null)
            {
                var dialogue = EzTalkEditor.instance.dialogue;
                speakInfo = textData.useSecondarySlot ? dialogue.secondarySpeaker.SpeakerKey : dialogue.primarySpeaker.SpeakerKey;
                if (textData.overrideSpeaker && textData.speakerOverride)
                {
                    speakInfo = textData.speakerOverride.SpeakerKey;
                }
                speakInfo += "| ";
            }

            if (isExpanded)
            {
                float oldWidth = EditorGUIUtility.labelWidth;

                displayInfoRect = new Rect(rect.x + borderWidth, rect.y + EzTalkEditor.instance.nodeHeight, rect.width - borderWidth * 2, heightOfNodeInfo);

                multiline = new GUIStyle(GUI.skin.label);
                multiline.wordWrap = true;
                multiline.richText = true;
                GUI.Box(new Rect(rect.x * zoom, rect.y * zoom, rect.width * zoom, (heightOfNodeInfo + borderWidth * 2 + rect.height) * zoom), "", style);
                GUILayout.BeginArea(new Rect(displayInfoRect.x * zoom, displayInfoRect.y * zoom, displayInfoRect.width * zoom, displayInfoRect.height * zoom));
                heightOfNodeInfo = rect.height * zoom;
                if (textData != null)
                {
                    GUILayout.Label(EzTalkEditor.instance.dialogue.GetFinalMessage(textData.message), multiline);
                    heightOfNodeInfo += multiline.CalcSize(new GUIContent(EzTalkEditor.instance.dialogue.GetFinalMessage(textData.message))).y / zoom;
                }

                SerializedObject serializedObject = new SerializedObject(EzTalkEditor.instance.dialogue);
                //going over all the connections and drawing them 
                for (int i = 0; i < nodeData.connections.Count; i++)
                {
                    if (nodeData.connections[i] >= EzTalkEditor.instance.dialogue.Connections.Count)
                    {
                        Debug.LogError("Connection does not exist");
                        nodeData.connections.RemoveAt(i);
                        continue;
                    }

                    BaseConnection myConnection = EzTalkEditor.instance.dialogue.Connections[nodeData.connections[i]];
                    GUILayout.Label(myConnection.title, multiline);
                    heightOfNodeInfo += multiline.CalcSize(new GUIContent(myConnection.title)).y / zoom;

                    if (myConnection is ResponseConnection responseConnection && !responseConnection.response.IsEmptyString())
                    {
                        GUILayout.Label(EzTalkEditor.instance.dialogue.GetFinalMessage(responseConnection.response), multiline);
                        heightOfNodeInfo += multiline.CalcSize(new GUIContent(EzTalkEditor.instance.dialogue.GetFinalMessage(responseConnection.response))).y / zoom;
                    }

                    //Setting up the height of the connection point. 
                    EzTalkEditor.instance.connections[nodeData.connections[i]].connectionStartPosition = (displayInfoRect.position + Vector2.up * (heightOfNodeInfo - rect.height) + Vector2.right * displayInfoRect.width) * zoom;
                }

                GUILayout.EndArea();
                EditorGUIUtility.labelWidth = oldWidth;
            }


            clickZone = new Rect(rect.x * zoom, rect.y * zoom, rect.width * zoom, rect.height * zoom);

            string title = (EzTalkEditor.instance.dialogue.startNode == position ? "START: " : "") + speakInfo + nodeData.title;

            GUI.Box(clickZone, title, EzTalkEditor.instance.nodeStyle);
            GUI.backgroundColor = pastColor;
        }

        private void SetLabelSize(string label)
        {
            GUIContent content = new GUIContent(label);

            GUIStyle style = GUI.skin.box;
            style.alignment = TextAnchor.MiddleCenter;

            // Compute how large the button needs to be.
            EditorGUIUtility.labelWidth = style.CalcSize(content).x;
        }

        public bool ProcessEvents(Event e, float zoom)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (((EzTalkEditor.instance.sideBarRect.Contains(e.mousePosition) && EzTalkEditor.instance.showSideBar) == false) && clickZone.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            if (isExpanded && EzTalkEditor.instance.lastSelectedNode != this)
                            {
                                GUI.FocusControl("Clear");
                                if (EzTalkEditor.instance.lastSelectedNode != null)
                                    EzTalkEditor.instance.lastSelectedNode.style = EzTalkEditor.instance.boxNodeStyle;
                                EzTalkEditor.instance.lastSelectedNode = this;
                                style = EzTalkEditor.instance.nodeStyle;
                            }
                            else
                            {
                                isExpanded = !isExpanded;
                                if (isExpanded)
                                {
                                    if (EzTalkEditor.instance.lastSelectedNode != null)
                                        EzTalkEditor.instance.lastSelectedNode.style = EzTalkEditor.instance.boxNodeStyle;
                                    EzTalkEditor.instance.lastSelectedNode = this;
                                    GUI.FocusControl("Clear");
                                    style = EzTalkEditor.instance.nodeStyle;
                                }
                                else
                                {
                                    EzTalkEditor.instance.lastSelectedNode = null;
                                    GUI.FocusControl("Clear");
                                    style = EzTalkEditor.instance.boxNodeStyle;
                                }
                            }
                        }
                        GUI.changed = true;
                    }

                    if (e.button == 1 && isExpanded && clickZone.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta, zoom);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Set start node"), false, OnClickAddAsStartNode);
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            OnRemoveNode(this);
        }

        private void OnClickAddAsStartNode()
        {
            EzTalkEditor.instance.dialogue.startNode = nodePosition;
        }
    }
}