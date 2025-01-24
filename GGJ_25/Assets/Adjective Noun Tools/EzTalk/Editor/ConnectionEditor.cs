using System;
using UnityEditor;
using UnityEngine;

namespace ANT.EzTalk
{
    public class ConnectionEditor
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        public Action<ConnectionEditor> OnClickRemoveConnection;
        public BaseConnection data;

        public Vector2 connectionStartPosition;

        public ConnectionEditor(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<ConnectionEditor> OnClickRemoveConnection, BaseConnection data)
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

                Vector2 buttonPos = (inPoint.rect.center + outPoint.rect.center) * 0.5f;
                //this deals with the remove button.
                if (Handles.Button(buttonPos, Quaternion.identity, 4, 8, Handles.RectangleHandleCap) && (EzTalkEditor.instance.sideBarRect.Contains(buttonPos) && EzTalkEditor.instance.showSideBar) == false)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}