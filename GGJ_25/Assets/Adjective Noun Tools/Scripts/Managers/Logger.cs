using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ANT
{
    [DefaultExecutionOrder(-3)]
    public class Logger : MonoBehaviour
    {
        public static Logger instance;
        public TextMeshProUGUI logField;
        public int storedMessges = 1;
        public List<string> messages = new List<string>();
        private void Awake()
        {
            if(instance == null)
                instance = this;
        }

        public static void Log(string message, Object context = null)
        {
            if (instance)
                instance._Log(message, context);
        }

        private void _Log(string message, Object context)
        {
            Display(message);
            Debug.Log(message, context);
        }

        public static void LogError(string message, Object context = null)
        {
            if(instance)
                instance._LogError(message, context);
        }

        private void _LogError(string message, Object context)
        {
            Display("ERROR: " + message);
            Debug.LogError(message, context);
        }

        public static void LogWarning(string message, Object context = null)
        {
            if (instance)
                instance._LogWarning(message, context);
        }

        private void _LogWarning(string message, Object context = null)
        {
            Display("WARNING: " + message);
            Debug.LogWarning(message, context);
        }

        private void Display(string newLog)
        {
            messages.Add(newLog);
            if (messages.Count > storedMessges)
                messages.RemoveAt(0);

            if (logField)
            {
                string displayMessage = messages[0];
                for (int i = 1; i < messages.Count; i++)
                {
                    displayMessage += "\n" + messages[i];
                }
                logField.text = displayMessage;
            }
        }

        public void ShowHideLogger()
        {
            if (!logField) return;
            logField.gameObject.SetActive(!logField.gameObject.activeSelf);
        }
    }
}
