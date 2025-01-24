using System.Collections;
using System.Collections.Generic;
//using Febucci.UI.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ANT.EzTalk
{
    //TODO: Add support for custom property drawer which supports all fields and shows just the one selected as the single field 
    [System.Serializable]
    public class EzTalkText
    {
        [SerializeField]
        private TextMeshProUGUI _textMeshProUGUI;
        [SerializeField]
        private Text _text;
        [SerializeField]
        private TextMeshPro _textMeshPro;

        //private TypewriterCore typewriter = null;

        public void Write(string message, bool useTypewrite = true)
        {
            /*
            if (typewriter && useTypewrite)
            {
                typewriter.ShowText(message);
            }
            else */if (_text)
            {
                _text.text = message;
            }
            else if (_textMeshPro)
            {
                _textMeshPro.text = message;
            }
            else if (_textMeshProUGUI)
            {
                _textMeshProUGUI.text = message;
            }
        }

        public void SkipTyping()
        {
            /*
            if (typewriter)
            {
                typewriter.SkipTypewriter();
            }
            */
        }

        public bool CanSkipTyping()
        {
            /*
            if (typewriter)
                return typewriter.isShowingText;
                */
            return false;
        }

        public void SetActive(bool state)
        {
            if (_text)
            {
                _text.gameObject.SetActive(state);
            }
            if (_textMeshPro)
            {
                _textMeshPro.gameObject.SetActive(state);
            }
            if (_textMeshProUGUI)
            {
                _textMeshProUGUI.gameObject.SetActive(state);
            }
        }

        public void SetupTextReferences(Transform parent)
        {
            _text = parent.GetComponentInChildren<Text>(true);
            _textMeshPro = parent.GetComponentInChildren<TextMeshPro>(true); ;
            _textMeshProUGUI = parent.GetComponentInChildren<TextMeshProUGUI>(true);
            //typewriter = parent.GetComponentInChildren<TypewriterCore>(true);
        }

        public void SetupTypewriter()
        {
            /*
            if (_text)
                typewriter = _text.GetComponent<TypewriterCore>();
            else if (_textMeshPro)
                typewriter = _textMeshPro.GetComponent<TypewriterCore>();
            else if (_textMeshProUGUI)
                typewriter = _textMeshProUGUI.GetComponent<TypewriterCore>();
            */
        }
    }
}