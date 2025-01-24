using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.EzTalk
{
    [CustomEditor(typeof(EzTalkCanvas))]
    public class EzTalkCanvasEditor : Editor
    {
        private void OnEnable()
        {
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}