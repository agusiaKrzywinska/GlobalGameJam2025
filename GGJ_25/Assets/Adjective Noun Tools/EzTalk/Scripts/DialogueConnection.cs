using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.ScriptableProperties;

namespace ANT.EzTalk
{
    [System.Serializable]
    public class DialogueConnection
    {
        //[HideInInspector]
        public string title;
        [HideInInspector]
        public int parent;
        [HideInInspector]
        public int nextNode;

        public string response;
        public ScriptableBool condition;
    }
}