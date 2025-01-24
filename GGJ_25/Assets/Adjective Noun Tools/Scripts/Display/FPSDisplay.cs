using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ANT
{
    public class FPSDisplay : MonoBehaviour
    {
        [SerializeField, Tooltip("The UI textbox which will display the FPS")]
        private TextMeshProUGUI textbox;

        // Update is called once per frame
        void Update()
        {
            //update the text to be current FPS.
            if (textbox)
                textbox.text = Mathf.Ceil(1f / Time.unscaledDeltaTime).ToString();
        }
    }
}