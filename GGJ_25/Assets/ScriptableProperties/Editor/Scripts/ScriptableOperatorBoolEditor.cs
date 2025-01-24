using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CustomEditor(typeof(ScriptableOperatorBool))]
    public class ScriptableOperatorBoolEditor : Editor
    {
        ScriptableOperatorBool _bool;
        SerializedProperty _onChanged;
        private void OnEnable()
        {
            _bool = (ScriptableOperatorBool)target;
            _onChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            EditorStyles.label.wordWrap = true;

            int newCount = Mathf.Max(1, EditorGUILayout.IntField(new GUIContent("Size", "How many operators are needed?"), _bool.Values.Count));
            int operatorCount = Mathf.Max(1, newCount - 1);
            //creating all the value instances
            while (newCount < _bool.Values.Count)
                _bool.Values.RemoveAt(_bool.Values.Count - 1);
            while (newCount > _bool.Values.Count)
                _bool.Values.Add(null);
            //creating all the operator instances
            while (operatorCount < _bool.Operations.Count)
                _bool.Operations.RemoveAt(_bool.Operations.Count - 1);
            while (operatorCount > _bool.Operations.Count)
                _bool.Operations.Add(ScriptableOperatorBool.Operator.AND);

            _bool.Values[0] = (ScriptableBool)EditorGUILayout.ObjectField("Value 1", _bool.Values[0], typeof(ScriptableBool), false);
            for (int i = 1; i < _bool.Values.Count; i++)
            {
                _bool.Operations[i-1] = (ScriptableOperatorBool.Operator)EditorGUILayout.EnumPopup("Operation", _bool.Operations[i-1]);
                _bool.Values[i] = (ScriptableBool)EditorGUILayout.ObjectField(_bool.Values[i], typeof(ScriptableBool), false);
            }

            if (_bool.Values.Count == 1)
            {
                _bool.Operations[0] = (ScriptableOperatorBool.Operator)EditorGUILayout.EnumPopup("Operation", _bool.Operations[0]);
                _bool.ValueConstant = EditorGUILayout.Toggle("Value 2 Constant", _bool.ValueConstant);
            }

            if (_bool.ReferenceSelf(new List<ScriptableProperty>()))
            {
                EditorGUILayout.HelpBox("Bool is referencing itself this will crash unity at runtime!", MessageType.Error);
            }
            else
            {
                EditorGUILayout.LabelField(_bool.GetEquation());
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Result " + _bool.GetValue());
            }

            EditorUtility.SetDirty(_bool);

            serializedObject.Update();
            EditorGUILayout.PropertyField(_onChanged);
            serializedObject.ApplyModifiedProperties();
        }
    }
}