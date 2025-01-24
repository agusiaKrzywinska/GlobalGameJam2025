using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ANT
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        [SerializeField, Tooltip("Text object the message will be written in.")]
        private TextMeshProUGUI text = null;
        [SerializeField, Tooltip("The background which resizes based on the text.")]
        private RectTransform background = null;
        [SerializeField, Tooltip("how many pixels of padding is around the text and background.")]
        private Vector2 padding = Vector2.one;

        protected override void OnSetup()
        {
            base.OnSetup();

            HideToolTip();
        }

        // Turns on the tooltip with showing the proper text and location and updates the background to match the text. 
        private void ShowToolTip(string tooltipString, Vector3 locationOfMessage)
        {
            gameObject.SetActive(true);
            text.text = tooltipString;

            text.rectTransform.localPosition = padding;
            background.sizeDelta = new Vector2(text.preferredWidth, text.preferredHeight) + padding * 2;
            transform.position = locationOfMessage;
        }

        // Hides the tooltip. 
        private void HideToolTip()
        {
            gameObject.SetActive(false);
        }

        // Static method to turn on the tooltip with showing the proper text and location and updates the background to match the text. 
        public static void ShowToolTip_Static(string tooltipString, Vector3 locationOfMessage)
        {
            instance.ShowToolTip(tooltipString, locationOfMessage);
        }

        // Static method to hide the tooltip. 
        public static void HideToolTip_Static()
        {
            instance.HideToolTip();
        }
    }
}