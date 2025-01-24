using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.EzTalk
{
    //[CustomPropertyDrawer(typeof(NodeSettings))]
    public class NodeSettingsEditor : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);
        }
        
    }
}