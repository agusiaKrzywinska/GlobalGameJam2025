using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace ANT.ScriptableProperties
{
    [AddComponentMenu("ANT/Scriptable Properties/Print Value")]
    public class PrintValue : MonoBehaviour
    {
        [SerializeField, Tooltip("Scriptable property value will be printed on update.")]
        private ScriptableProperty _scriptable = null;
        private ScriptablePropertyCast cast;

        Text text = null;
        TextMeshProUGUI textMesh = null;
        public string format = "";

        private void Awake()
        {
            //finding the text components. 
            textMesh = GetComponent<TextMeshProUGUI>();
            text = GetComponent<Text>();

            cast = new ScriptablePropertyCast(_scriptable);
        }

        void Update()
        {
            //print the value of the scriptable object in the text component.    
            if (text)
            {
                text.text = cast.GetString();
            }
            if (textMesh)
            {
                textMesh.text = cast.GetString();
            }
        }
    }
}

