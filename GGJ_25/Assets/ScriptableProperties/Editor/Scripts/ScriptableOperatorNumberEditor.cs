using UnityEditor;
using UnityEngine;

namespace ANT.ScriptableProperties
{
    [CustomEditor(typeof(ScriptableOperatorNumber))]
    public class ScriptableOperatorNumberEditor : Editor
    {
        ScriptableOperatorNumber _number;
        SerializedProperty _onChanged;

        private void OnEnable()
        {
            _number = (ScriptableOperatorNumber)target;
            _onChanged = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            EditorStyles.label.wordWrap = true;

            _number.Value1 = (ScriptableProperty)EditorGUILayout.ObjectField("Value 1", _number.Value1, typeof(ScriptableProperty), false);
            if (_number.Value1 && _number.GetTypeOfCast == ScriptableType.NONE || _number.GetTypeOfCast == ScriptableType.BOOL || _number.GetTypeOfCast == ScriptableType.STRING)
            {
                EditorGUILayout.HelpBox("Unsupported Scriptable Property Added!", MessageType.Error);
            }

            _number.operation = (ScriptableOperatorNumber.Operator)EditorGUILayout.EnumPopup("Operation", _number.operation);

            if (_number.GetTypeOfCast != ScriptableType.NONE || _number.GetTypeOfCast != ScriptableType.BOOL || _number.GetTypeOfCast != ScriptableType.STRING)
            {
                _number.Value2 = (ScriptableProperty)EditorGUILayout.ObjectField("Value 2", _number.Value2, typeof(ScriptableProperty), false);
                if (_number.Value2 && _number.GetTypeOfValue2 == ScriptableType.NONE || _number.GetTypeOfValue2 == ScriptableType.BOOL || _number.GetTypeOfValue2 == ScriptableType.STRING)
                {
                    EditorGUILayout.HelpBox("Unsupported Scriptable Property Added!", MessageType.Error);
                }
                else if(!_number.Value2)
                {
                    switch (_number.GetTypeOfCast)
                    {
                        case ScriptableType.INT:
                            _number.Value2ConstantInt = EditorGUILayout.IntField("Value 2 Constant", _number.Value2ConstantInt);
                            break;
                        case ScriptableType.FLOAT:
                            _number.Value2ConstantFloat = EditorGUILayout.FloatField("Value 2 Constant", _number.Value2ConstantFloat);
                            break;
                        case ScriptableType.LONG:
                            _number.Value2ConstantLong = EditorGUILayout.LongField("Value 2 Constant", _number.Value2ConstantLong);
                            break;
                    }
                }

            }

            EditorGUILayout.LabelField(_number.GetEquation());

            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("Result " + _number.GetValue());
            }

            EditorUtility.SetDirty(_number);
            
            serializedObject.Update();
            EditorGUILayout.PropertyField(_onChanged);
            serializedObject.ApplyModifiedProperties();
        }
    }
}