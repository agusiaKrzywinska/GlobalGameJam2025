using System;
using UnityEngine;

namespace ANT.EzTalk
{
    public enum ConnectionPointType { In, Out }

    public class ConnectionPoint
    {
        public Rect rect;
        public ConnectionPointType type;
        public NodeEditor node;
        public GUIStyle style;
        public Action<ConnectionPoint> OnClickConnectionPoint;

        private const float offset = 2f;

        public ConnectionPoint(NodeEditor node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint)
        {
            this.node = node;
            this.type = type;
            this.style = style;
            this.OnClickConnectionPoint = OnClickConnectionPoint;
            rect = new Rect(0, 0, 10f, 20f);
        }

        public void Draw(float zoom)
        {
            rect.y = node.rect.y * zoom + (node.rect.height * zoom * 0.5f) - rect.height * 0.5f;

            switch (type)
            {
                case ConnectionPointType.In:
                    rect.x = node.rect.x * zoom - rect.width + offset;
                    break;

                case ConnectionPointType.Out:
                    rect.x = node.rect.x * zoom + node.rect.width * zoom - offset;
                    break;
            }

            if (GUI.Button(rect, "", style) && (EzTalkEditor.instance.sideBarRect.Contains(rect.position) && EzTalkEditor.instance.showSideBar) == false)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}