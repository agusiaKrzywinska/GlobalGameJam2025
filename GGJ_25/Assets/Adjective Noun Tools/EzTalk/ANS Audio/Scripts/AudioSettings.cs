using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{

    [DefaultExecutionOrder(-10)]
    public class AudioSettings : Singleton<AudioSettings>
    {
        [SerializeField]
        private bool showLogs = true;
        [SerializeField]
        private bool showWarnings = true;
        
        public static bool ShowWarnings => instance.showWarnings;
        public static bool ShowLogs => instance.showLogs;
    }
}