using System;
using UnityEditor;
using UnityEngine;

namespace ANT.EzTalk
{
    [CustomEditor(typeof(Dialogue))]
    public class SpeakerEditor : Editor
    {
        private Dialogue dialogue;

        private void OnEnable()
        {
            dialogue = (Dialogue)target;
        }

        public override void OnInspectorGUI()
        {
            DrawSpeakerInfo();
            if (GUILayout.Button("Open Editor"))
            {
                EditorWindow.GetWindow(typeof(EzTalkEditor), false, "EzTalkEditor");
            }

        }

        public void DrawSpeakerInfo()
        {
            base.OnInspectorGUI();
        }
    }
}
