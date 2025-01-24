using ANT.ScriptableProperties;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.EzTalk
{
    public class EzTalkEditor : EditorWindow
    {
        public static EzTalkEditor instance;

        public List<NodeEditor> nodes;
        public List<ConnectionEditor> connections;

        private float nodeWidth = 200f;
        public float nodeHeight = 50f;

        public GUIStyle nodeStyle, boxNodeStyle;
        public GUIStyle inPointStyle;
        public GUIStyle outPointStyle;
        private Texture2D addButtonTexture, minusButtonTexture;
        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 gridOffset;
        private Vector2 drag;

        public Dialogue dialogue;
        private Dialogue selectedDialogue;

        public bool showSideBar = true;

        private string tempParameterName;
        private Parameter tempParameterType;

        private float zoom = 1f;
        public NodeEditor lastSelectedNode = null;

        private Vector2 scrollbarParameters = Vector2.zero;
        private Vector2 scrollbarContent = Vector2.zero;

        public Rect sideBarRect;

        [MenuItem("Tools/Adjective Noun Tools/EzTalk Editor")]
        private static void OpenWindow()
        {
            EzTalkEditor window = GetWindow<EzTalkEditor>();
            window.titleContent = new GUIContent("EzTalkEditor");
        }

        public void UpdateSelection()
        {
            ParameterChanger changer = Selection.activeGameObject ? Selection.activeGameObject.GetComponent<ParameterChanger>() : null;
            Dialogue dia = Selection.activeObject as Dialogue;
            if (dia)
            {
                selectedDialogue = dia;
                ResetEditorInfo();
            }
            else if (changer)
            {
                selectedDialogue = changer.dialogue;
                ResetEditorInfo();
            }
        }

        private void ResetEditorInfo()
        {
            lastSelectedNode = null;
            GUI.FocusControl("Clear");
        }

        private void OnEnable()
        {
            instance = this;

            UpdateSelection();

            sideBarRect = new Rect(0, 0, position.width / 4, position.height);

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Tools/Adjective Noun Tools/EzTalk/Editor/nodeBase.png", typeof(Texture2D));//EditorGUIUtility.Load("builtin skins/lightskin/images/node1.png") as Texture2D;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);

            boxNodeStyle = new GUIStyle();
            boxNodeStyle.normal.background = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Tools/Adjective Noun Tools/EzTalk/Editor/boxBase.png", typeof(Texture2D));//EditorGUIUtility.Load("builtin skins/lightskin/images/node1 on.png") as Texture2D;
            boxNodeStyle.alignment = TextAnchor.UpperCenter;
            boxNodeStyle.border = new RectOffset(12, 12, 12, 12);

            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            inPointStyle.border = new RectOffset(4, 4, 12, 12);

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            outPointStyle.border = new RectOffset(4, 4, 12, 12);

            addButtonTexture = EditorGUIUtility.FindTexture("d_Toolbar Plus");
            minusButtonTexture = EditorGUIUtility.FindTexture("d_Toolbar Minus");
        }

        private void OnSelectionChange()
        {
            UpdateSelection();
        }

        private void OnFocus()
        {
            UpdateSelection();
        }

        private void OnGUI()
        {
            //checking if selected is a speaker.
            if (selectedDialogue)
            {
                UpdateDialogue();
                if (!dialogue)
                {
                    selectedDialogue = dialogue;
                    UpdateNodeData();
                }
                DrawEzTalk();
                DrawBottomBar();

                ProcessNodeEvents(Event.current);
                ProcessEvents(Event.current);
            }
            else
            {
                dialogue = null;
                EditorGUILayout.HelpBox("Please Select a Dialogue", MessageType.Warning);
            }

            if (GUI.changed)
            {
                if (dialogue != null)
                    EditorUtility.SetDirty(dialogue);
                Repaint();
            }
        }

        private void DrawBottomBar()
        {
            BeginWindows();
            if (showSideBar)
                sideBarRect = GUI.Window(1, new Rect(0, 0, position.width / 4, position.height), DrawSidePanel, "");
            EndWindows();

            string state = showSideBar ? "Hide" : "Show";
            showSideBar = EditorGUILayout.Toggle($"{state} Sidebar", showSideBar);

            GUILayout.BeginArea(new Rect(showSideBar ? position.width / 4 : 0, position.height - 20f, showSideBar ? (position.width / 4 * 3) : position.width, 20f));
            GUILayout.BeginHorizontal();
            if (lastSelectedNode == null && GUILayout.Button("Centre Start Node"))
            {
                CentreOnNode(dialogue.startNode);
            }
            else if (lastSelectedNode != null && GUILayout.Button("Centre on Current Selected Node"))
            {
                CentreOnNode(lastSelectedNode.nodePosition);
            }
            zoom = EditorGUILayout.Slider("Zoom", zoom, 0.35f, 2f);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void UpdateNodeData()
        {
            nodes = new List<NodeEditor>();
            connections = new List<ConnectionEditor>();

            for (int i = 0; i < dialogue.Nodes.Count; i++)
            {
                nodes.Add(new NodeEditor(dialogue.Nodes[i].positionInEditor, nodeWidth, nodeHeight, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, dialogue.Nodes[i]));
            }

            for (int i = 0; i < dialogue.Connections.Count; i++)
            {
                if (dialogue.Connections[i].nextNode >= nodes.Count)
                {
                    Debug.LogError($"Deleted connection {dialogue.Connections[i].title} from not having a next node");
                    dialogue.Connections.RemoveAt(i);
                    continue;
                }
                if (dialogue.Connections[i].parent >= nodes.Count)
                {
                    Debug.LogError($"Deleted connection {dialogue.Connections[i].title} from not having a parent");
                    dialogue.Connections.RemoveAt(i);
                    continue;
                }

                ConnectionPoint starting = nodes[dialogue.Connections[i].nextNode].inPoint;
                ConnectionPoint ending = nodes[dialogue.Connections[i].parent].outPoint;
                connections.Add(new ConnectionEditor(starting, ending, OnClickRemoveConnection, dialogue.Connections[i]));
            }
            GUI.changed = true;

            UpdateAllNodes();
        }

        private void UpdateDialogue()
        {
            //this will only happen once
            if (dialogue != selectedDialogue)
            {
                dialogue = selectedDialogue;
                UpdateNodeData();
            }
        }

        private void DrawSidePanel(int windowID)
        {
            //drawing the new parameter. 
            //draw all the parameters
            EditorGUILayout.BeginHorizontal();
            tempParameterName = EditorGUILayout.TextField(tempParameterName);
            tempParameterType = (Parameter)EditorGUILayout.EnumPopup(tempParameterType);
            if (GUILayout.Button(addButtonTexture))
            {
                //add parameter
                if (dialogue.AddParameter(tempParameterName, new EzTalkParameter(tempParameterType)))
                {
                    tempParameterName = "";
                    Debug.Log("Parameter added");
                }
                else
                {
                    Debug.LogError("Parameter could not be added");
                }
            }
            EditorGUILayout.EndHorizontal();

            scrollbarParameters = EditorGUILayout.BeginScrollView(scrollbarParameters, false, false);
            foreach (EzTalkParameter parameter in dialogue.Parameters)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(parameter.name);
                switch (parameter.parameterType)
                {
                    case Parameter.Int:
                        parameter.SetValue(EditorGUILayout.IntField(parameter.Int));
                        break;
                    case Parameter.Bool:
                        parameter.SetValue(EditorGUILayout.Toggle(parameter.Bool));
                        break;
                    case Parameter.String:
                        parameter.SetValue(EditorGUILayout.TextField(parameter.String));
                        break;
                    case Parameter.Float:
                        parameter.SetValue(EditorGUILayout.FloatField(parameter.Float));
                        break;
                    case Parameter.ScriptableProperty:
                        parameter.ScriptableProperty = (ScriptableProperty)EditorGUILayout.ObjectField(parameter.ScriptableProperty, typeof(ScriptableProperty), false);
                        break;
                }

                bool remove = GUILayout.Button(minusButtonTexture);
                EditorGUILayout.EndHorizontal();
                if (remove)
                {
                    //remove parameter
                    if (dialogue.RemoveParameter(parameter.name))
                    {
                        Debug.Log("Parameter removed");
                    }
                    else
                    {
                        Debug.LogError("Parameter could not be removed");
                    }
                }
            }
            EditorGUILayout.EndScrollView();

            EzTalkStyles.DrawUILine(new Color(0f, 0f, 0f, 0.5f));
            //drawing node settings
            EditorGUILayout.LabelField("Node Information");
            //title bar
            if (lastSelectedNode != null)
            {
                scrollbarContent = EditorGUILayout.BeginScrollView(scrollbarContent, false, false);
                lastSelectedNode.DrawNodeSidebarInfo();
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.HelpBox("No node selected", MessageType.Info);
            }
        }

        private void DrawEzTalk()
        {
            DrawGrid(20 * zoom, 0.2f, Color.grey);
            DrawGrid(100 * zoom, 0.4f, Color.grey);

            UpdateAllNodes();

            DrawNodes(false);
            DrawConnections();
            DrawNodes(true);

            DrawConnectionLine(Event.current);
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            gridOffset += drag * 0.5f;
            Vector3 newOffset = new Vector3(gridOffset.x % gridSpacing, gridOffset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes(bool isOpened)
        {
            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (nodes[i].isExpanded == isOpened && nodes[i].IsOnScreen(position, zoom))
                    {
                        nodes[i].Draw(i, zoom);
                    }
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    if (!CullConnection(connections[i]))
                        connections[i].Draw(IsExpanded(connections[i]));
                }
            }
        }

        private bool CullConnection(ConnectionEditor connection)
        {
            return !nodes[connection.data.parent].IsOnScreen(position, zoom) && !nodes[connection.data.nextNode].IsOnScreen(position, zoom);
        }

        private bool IsExpanded(ConnectionEditor connection)
        {
            return nodes[connection.data.parent].isExpanded;
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (selectedInPoint != null || selectedOutPoint != null)
                            DrawNodeMenu(e.mousePosition);
                        else
                            ClearConnectionSelection();
                    }
                    if (e.button == 1)
                    {
                        if (selectedInPoint != null || selectedOutPoint != null)
                            ClearConnectionSelection();
                        else
                            DrawNodeMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 || e.button == 2)
                    {
                        OnDrag(e.delta);
                    }
                    break;
                case EventType.ScrollWheel:
                    zoom = Mathf.Clamp(zoom - e.delta.y * 0.01f, 0.35f, 2f);
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e, zoom);

                    //updating GUI
                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void DrawNodeMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Text Node"), false, () => OnClickAddNode(1, mousePosition));
            genericMenu.AddItem(new GUIContent("Add Node"), false, () => OnClickAddNode(0, mousePosition));
            genericMenu.ShowAsContext();
        }

        private void DrawConnectionMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add Connection"), false, () => CreateConnection(0));
            genericMenu.AddItem(new GUIContent("Add Response Connection"), false, () => CreateConnection(1));
            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta, zoom);
                }
            }

            GUI.changed = true;
        }

        private void OnClickAddNode(int type, Vector2 mousePosition)
        {
            if (nodes == null)
            {
                nodes = new List<NodeEditor>();
            }

            BaseNode temp;
            switch (type)
            {
                case 1:
                    temp = new TextNode
                    {
                        settings = dialogue.defaultSettings,
                        message = ""
                    };
                    break;
                case 0:
                default:
                    temp = new BaseNode();
                    break;
            }
            var nodeEditior = new NodeEditor(mousePosition / zoom, nodeWidth, nodeHeight, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, temp);
            nodes.Add(nodeEditior);
            dialogue.Nodes.Add(temp);

            if (selectedInPoint != null)
            {
                selectedOutPoint = nodeEditior.outPoint;
                CreateConnection(0);
            }
            else if (selectedOutPoint != null)
            {
                selectedInPoint = nodeEditior.inPoint;
                CreateConnection(0);
            }


            UpdateAllNodes();
        }

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    DrawConnectionMenu();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    DrawConnectionMenu();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveConnection(ConnectionEditor connection)
        {
            //remove the connection from the node
            nodes[connection.data.parent].nodeData.connections.Remove(FindConnectionID(connection.data));

            dialogue.Connections.Remove(connection.data);
            connections.Remove(connection);

            UpdateAllNodes();
        }

        private void CreateConnection(int type)
        {
            if (connections == null)
            {
                connections = new List<ConnectionEditor>();
            }

            BaseConnection temp;
            switch (type)
            {
                case 1:
                    temp = new ResponseConnection
                    {
                        nextNode = FindNodeID(selectedInPoint.node.nodeData),
                        parent = FindNodeID(selectedOutPoint.node.nodeData),
                        title = selectedInPoint.node.nodeData.title + " - " + selectedOutPoint.node.nodeData.title,
                        response = ""
                    };
                    break;
                case 0:
                default:
                    temp = new BaseConnection
                    {
                        nextNode = FindNodeID(selectedInPoint.node.nodeData),
                        parent = FindNodeID(selectedOutPoint.node.nodeData),
                        title = selectedInPoint.node.nodeData.title + " - " + selectedOutPoint.node.nodeData.title,
                    };
                    break;
            }
            //add the connection to the node
            nodes[temp.parent].nodeData.connections.Add(connections.Count);

            //setup the inPoint and outPoint to be the nodes. 
            connections.Add(new ConnectionEditor(selectedInPoint, selectedOutPoint, OnClickRemoveConnection, temp));
            dialogue.Connections.Add(temp);

            ClearConnectionSelection();
            UpdateAllNodes();
        }

        private int FindNodeID(BaseNode node)
        {
            for (int i = 0; i < dialogue.Nodes.Count; i++)
            {
                if (dialogue.Nodes[i] == node)
                    return i;
            }
            return -1;
        }

        private int FindConnectionID(BaseConnection connection)
        {
            for (int i = 0; i < dialogue.Connections.Count; i++)
            {
                if (dialogue.Connections[i] == connection)
                    return i;
            }
            return -1;
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }

        private void OnClickRemoveNode(NodeEditor node)
        {

            //fixes the delete node bug
            lastSelectedNode = null;
            //remove all connections linked to the node to remove. 
            if (connections != null)
            {
                for (int i = connections.Count - 1; i >= 0; i--)
                {
                    if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                    {
                        OnClickRemoveConnection(connections[i]);
                    }
                }
            }
            //modify all connections to fix pathing if you delete a node after the current node was made. 
            int position = FindNodeID(node.nodeData);
            for (int i = 0; i < dialogue.Connections.Count; i++)
            {
                if (dialogue.Connections[i].nextNode > position)
                {
                    dialogue.Connections[i].nextNode--;
                }
                if (dialogue.Connections[i].parent > position)
                {
                    dialogue.Connections[i].parent--;
                }
            }
            //if you remove a node before start node the start node will still point to the same node
            if (position < dialogue.startNode)
            {
                dialogue.startNode--;
            }

            dialogue.Nodes.Remove(node.nodeData);
            nodes.Remove(node);
            UpdateAllNodes();
        }

        private void UpdateAllNodes()
        {
            if (nodes == null)
            {
                UpdateNodeData();
                return;
            }

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].nodeData.connections = new List<int>();

                for (int j = 0; j < nodes[i].nodeData.connections.Count; j++)
                {
                    if (nodes[i].nodeData.connections[j] >= connections.Count)
                    {
                        Debug.Log("Removed connection");
                        nodes[i].nodeData.connections.RemoveAt(j);
                    }
                }
            }

            UpdateAllConnections();

            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].nodeData.SetUpIfWaitForResponseNode(dialogue);
            }
        }

        private void UpdateAllConnections()
        {
            if (connections == null)
            {
                UpdateNodeData();
                return;
            }
            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].data.nextNode >= nodes.Count)
                {
                    Debug.Log("Removed connection leading to dead node");
                    connections.RemoveAt(i);
                    continue;
                }
                connections[i].data.title = nodes[connections[i].data.parent].nodeData.title + " - " + nodes[connections[i].data.nextNode].nodeData.title;
                nodes[connections[i].data.parent].nodeData.connections.Add(i);
            }
        }

        private void CentreOnNode(int nodeID)
        {
            var offset = dialogue.Nodes[nodeID].positionInEditor * zoom - position.size / 2;
            OnDrag(-offset);

            DrawEzTalk();
        }
    }
}