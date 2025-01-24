using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT
{
    [CreateAssetMenu(fileName = "Version", menuName = "ANT/Versioning")]
    public class Version : ScriptableObject
    {
        [SerializeField]
        private int releaseVersion, majorVersion, minorVersion;

        [SerializeField]
        private string versionEndInfo = "";

        [SerializeField]
        private int bundleVersion;

        public string version
        {
            get
            {
                string temp = $"{releaseVersion}.{majorVersion}.{minorVersion}";
                if (versionEndInfo != null && versionEndInfo.Trim() != "")
                {
                    temp += $"-{ versionEndInfo}";
                }
                return temp;
            }
        }


#if UNITY_EDITOR
        private void UpdateMinorVersion()
        {
            minorVersion++;
            UpdateEditorStuff();
        }
        private void UpdateMajorVersion()
        {
            minorVersion = 0;
            majorVersion++;
            UpdateEditorStuff();
        }
        private void UpdateReleaseVersion()
        {
            minorVersion = 0;
            majorVersion = 0;
            releaseVersion++;
            UpdateEditorStuff();
        }
        private void UpdatePublishingRelease()
        {
            bundleVersion++;
            UpdateEditorStuff();
        }
        private void UpdateEditorStuff()
        {
            UnityEditor.PlayerSettings.bundleVersion = version;

            UnityEditor.PlayerSettings.Android.bundleVersionCode = bundleVersion;
            UnityEditor.PlayerSettings.iOS.buildNumber = bundleVersion.ToString();

            UnityEditor.EditorUtility.SetDirty(this);
            
        }
#endif
    }
}