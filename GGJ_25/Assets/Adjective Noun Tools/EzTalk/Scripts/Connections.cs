using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ANT.ScriptableProperties;
using UnityEngine.Events;

namespace ANT.EzTalk
{
    [System.Serializable]
    public class BaseConnection
    {
        //[HideInInspector]
        public string title;
        [HideInInspector]
        public int parent;
        [HideInInspector]
        public int nextNode;

        public ScriptableBool condition;

        public UnityEvent onTrigger;

        public virtual void TriggerConnection()
        {
            onTrigger.Invoke();
        }

        public virtual bool CanShow => !condition || condition.GetValue();

        public bool ShowReply => CanShow && this is ResponseConnection;
    }

    [System.Serializable]
    public class ResponseConnection : BaseConnection
    {
        public string response;
    }
}