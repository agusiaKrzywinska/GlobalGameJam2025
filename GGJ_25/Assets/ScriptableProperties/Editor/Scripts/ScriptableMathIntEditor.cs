using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CustomEditor(typeof(ScriptableMathInt))]
    public class ScriptableMathIntEditor : Editor
    {
        ScriptableMathInt _int;
        SerializedProperty _onChanged;
        private void OnEnable()
        {
            _int = (ScriptableMathInt)target;
            _onChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            EditorStyles.label.wordWrap = true;

            int newCount = Mathf.Max(2, EditorGUILayout.IntField(new GUIContent("Size", "How many operators are needed?"), _int.Values.Count));
            int operatorCount = Mathf.Max(1, newCount - 1);
            //creating all the value instances
            while (newCount < _int.Values.Count)
                _int.Values.RemoveAt(_int.Values.Count - 1);
            while (newCount > _int.Values.Count)
                _int.Values.Add(new ScriptableMathInt.ScriptableValue());
            //creating all the operator instances
            while (operatorCount < _int.Operations.Count)
                _int.Operations.RemoveAt(_int.Operations.Count - 1);
            while (operatorCount > _int.Operations.Count)
                _int.Operations.Add(ScriptableMathInt.Operator.ADD);

            for (int i = 0; i < _int.Values.Count; i++)
            {
                if(i != 0)
                    _int.Operations[i - 1] = (ScriptableMathInt.Operator)EditorGUILayout.EnumPopup(new GUIContent("Operation", "The operation runs between two values. All operations are running sequentially starting with the first value."), _int.Operations[i - 1]);
                if(!_int.Values[i].hideScriptableField)
                _int.Values[i].cast.SetProperty((ScriptableProperty)EditorGUILayout.ObjectField($"Value {i + 1}",_int.Values[i].cast._property, typeof(ScriptableProperty), false));
                if (_int.Values[i].cast._property && (_int.GetTypeAt(i) == ScriptableType.NONE || _int.GetTypeAt(i) == ScriptableType.BOOL || _int.GetTypeAt(i) == ScriptableType.STRING))
                {
                    EditorGUILayout.HelpBox("Unsupported Scriptable Property Added!", MessageType.Error);
                }
                if(!_int.Values[i].cast._property)
                {
                    EditorGUILayout.BeginHorizontal();
                    _int.Values[i].constant = EditorGUILayout.IntField($"Value {i + 1}", _int.Values[i].constant);
                    _int.Values[i].hideScriptableField = EditorGUILayout.Toggle(new GUIContent("Hide Scriptable Field", $"This hides the scriptable property field of Value {i + 1}."), _int.Values[i].hideScriptableField);
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (_int.ReferenceSelf(new List<ScriptableProperty>()))
            {
                EditorGUILayout.HelpBox("Int is referencing itself this will crash unity at runtime!", MessageType.Error);
            }
            else
            {
                EditorGUILayout.LabelField(_int.GetEquation());
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Result " + _int.GetValue());
            }

            EditorUtility.SetDirty(_int);
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(_onChanged);
            serializedObject.ApplyModifiedProperties();

        }
    }
}