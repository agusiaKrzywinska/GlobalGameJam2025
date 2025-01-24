using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace ANT
{
    public class VersionDisplayer : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textUI;
        [SerializeField]
        private TextMeshPro text;
        [SerializeField]
        private Version version;

        private void Start()
        {
            if (textUI)
                textUI.text = version.version;
            if (text)
                text.text = version.version;
        }
    }
}