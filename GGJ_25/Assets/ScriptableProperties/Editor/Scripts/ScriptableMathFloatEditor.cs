using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CustomEditor(typeof(ScriptableMathFloat))]
    public class ScriptableMathFloatEditor : Editor
    {
        ScriptableMathFloat _float;
        private void OnEnable()
        {
            _float = (ScriptableMathFloat)target;
        }

        public override void OnInspectorGUI()
        {
            EditorStyles.label.wordWrap = true;

            int newCount = Mathf.Max(2, EditorGUILayout.IntField(new GUIContent("Size", "How many operators are needed?"), _float.Values.Count));
            int operatorCount = Mathf.Max(1, newCount - 1);
            //creating all the value instances
            while (newCount < _float.Values.Count)
                _float.Values.RemoveAt(_float.Values.Count - 1);
            while (newCount > _float.Values.Count)
                _float.Values.Add(new ScriptableMathFloat.ScriptableValue());
            //creating all the operator instances
            while (operatorCount < _float.Operations.Count)
                _float.Operations.RemoveAt(_float.Operations.Count - 1);
            while (operatorCount > _float.Operations.Count)
                _float.Operations.Add(ScriptableMathFloat.Operator.ADD);

            for (int i = 0; i < _float.Values.Count; i++)
            {
                if (i != 0)
                    _float.Operations[i - 1] = (ScriptableMathFloat.Operator)EditorGUILayout.EnumPopup(new GUIContent("Operation", "The operation runs between two values. All operations are running sequentially starting with the first value."), _float.Operations[i - 1]);
                if (!_float.Values[i].hideScriptableField)
                    _float.Values[i].cast.SetProperty((ScriptableProperty)EditorGUILayout.ObjectField($"Value {i + 1}", _float.Values[i].cast._property, typeof(ScriptableProperty), false));
                if (_float.Values[i].cast._property && (_float.GetTypeAt(i) == ScriptableType.NONE || _float.GetTypeAt(i) == ScriptableType.BOOL || _float.GetTypeAt(i) == ScriptableType.STRING))
                {
                    EditorGUILayout.HelpBox("Unsupported Scriptable Property Added!", MessageType.Error);
                }
                if (!_float.Values[i].cast._property)
                {
                    EditorGUILayout.BeginHorizontal();
                    _float.Values[i].constant = EditorGUILayout.FloatField($"Value {i + 1}", _float.Values[i].constant);
                    _float.Values[i].hideScriptableField = EditorGUILayout.Toggle(new GUIContent("Hide Scriptable Field", $"This hides the scriptable property field of Value {i + 1}."), _float.Values[i].hideScriptableField);
                    EditorGUILayout.EndHorizontal();
                }
            }

            if (_float.ReferenceSelf(new List<ScriptableProperty>()))
            {
                EditorGUILayout.HelpBox("Int is referencing itself this will crash unity at runtime!", MessageType.Error);
            }
            else
            {
                EditorGUILayout.LabelField(_float.GetEquation());
            }

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Result " + _float.GetValue());
            }

            EditorUtility.SetDirty(_float);
        }
    }
}