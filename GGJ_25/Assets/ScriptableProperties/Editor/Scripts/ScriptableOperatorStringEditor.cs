using UnityEditor;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CustomEditor(typeof(ScriptableOperatorString))]
    public class ScriptableOperatorStringEditor : Editor
    {
        ScriptableOperatorString _string;
        SerializedProperty _onChanged;
        private void OnEnable()
        {
            _string = (ScriptableOperatorString)target;
            _onChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            EditorStyles.label.wordWrap = true;

            _string.Value1 = (ScriptableString)EditorGUILayout.ObjectField("Value 1",_string.Value1, typeof(ScriptableString), false);
            _string.operation =  (ScriptableOperatorString.Operator) EditorGUILayout.EnumPopup("Operation", _string.operation);
            _string.Value2 = (ScriptableString)EditorGUILayout.ObjectField("Value 2", _string.Value2, typeof(ScriptableString), false);
            if (!_string.Value2)
                _string.Value2Constant = EditorGUILayout.TextField("Value 2 Constant", _string.Value2Constant);
            EditorGUILayout.LabelField(_string.GetEquation());

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Result " + _string.GetValue());
            }

            EditorUtility.SetDirty(_string);
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(_onChanged);
            serializedObject.ApplyModifiedProperties();
        }
    }
}