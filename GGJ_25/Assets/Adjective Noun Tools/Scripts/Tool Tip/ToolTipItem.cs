using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ANT
{
    public class ToolTipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Tooltip("The message you want to show when you mouse over this tooltip")]
        private string message;
        [SerializeField, Tooltip("The location of where the tooltip will appear.")]
        private Transform locationOfMessage;
        private bool isUI;

        public string Message { get => message; set => message = value; }
        public Transform LocationOfMessage => locationOfMessage;

        private void Awake()
        {
            //check to see if this is on a UI element to convert to screen space before placing it. 
            isUI = LocationOfMessage.GetComponent<RectTransform>();
        }

        public void OnMouseOver()
        {
            CheckIfPressingForTooltip();
        }

        public void OnMouseExit()
        {
            TooltipManager.HideToolTip_Static();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            CheckIfPressingForTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.HideToolTip_Static();
        }

        /// <summary>
        /// check to confirm that the tooltip is being pressed it show it.
        /// </summary>
        private void CheckIfPressingForTooltip()
        {
            if (Input.GetMouseButton(0))
            {
                // setup the position of the text. 
                Vector3 pos = isUI ? LocationOfMessage.position : Camera.main.WorldToScreenPoint(LocationOfMessage.position);
                TooltipManager.ShowToolTip_Static(Message, pos);
            }
            else
            {
                TooltipManager.HideToolTip_Static();
            }
        }

    }
}