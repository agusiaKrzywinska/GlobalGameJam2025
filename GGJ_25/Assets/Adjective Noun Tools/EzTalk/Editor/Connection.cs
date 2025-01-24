using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ANT.EzTalk
{
    public class Connection
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        public Action<Connection> OnClickRemoveConnection;
        public DialogueConnection data;

        public Vector2 connectionStartPosition;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection, DialogueConnection data)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
            this.data = data;
        }

        public void Draw(bool isSelected = false)
        {
            //if (inPoint == null || outPoint == null)
            //    return;

            //drawing expanded connections
            if (isSelected)
            {
                Handles.DrawBezier(
                inPoint.rect.center,
                connectionStartPosition,
                inPoint.rect.center + Vector2.left * 50f,
                connectionStartPosition - Vector2.left * 50f,
                Color.blue,
                null,
                2f
            );
            }
            else
            {
                Handles.DrawBezier(
                    inPoint.rect.center,
                    outPoint.rect.center,
                    inPoint.rect.center + Vector2.left * 50f,
                    outPoint.rect.center - Vector2.left * 50f,
                    Color.blue,
                    null,
                    2f
                );

                //this deals with the remove button.
                if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleHandleCap))
                {
                    OnClickRemoveConnection(this);
                }
            }
            
        }
    }
}