using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ANT.Audio
{
    [CreateAssetMenu(fileName = "New Volume", menuName = "Adjective Noun Tools/Audio/Volume")]
    public class Volume : ScriptableObject
    {
        public string parameterName = "";
        public float currentVolume = 0f;
        public float minVolume = -80f;
        public float maxVolume = 0f;
        public bool isMuted = false;
        public float fadeInTime = 1f;
        public float fadeOutTime = 1f;
        public AudioMixer mixer = null;

        public void PrintVolume(TMPro.TextMeshProUGUI text)
        {
            if(text)
            {
                text.text = ((int)(Mathf.InverseLerp(minVolume, maxVolume, currentVolume) * 100)).ToString();
            }
        }

        public void PrintVolume(UnityEngine.UI.Text text)
        {
            if (text)
            {
                text.text = ((int)(Mathf.InverseLerp(minVolume, maxVolume, currentVolume) * 100)).ToString();
            }
        }

        private void UpdateVolume(float newVol)
        {
            currentVolume = Mathf.Log10(newVol) * 20;
            UpdateMixer();
        }

        private void UpdateMute(bool muted)
        {
            isMuted = muted;
            UpdateMixer();
        }

        public void UpdateMixer()
        {
            mixer.SetFloat(parameterName, isMuted ? minVolume : currentVolume);
        }

        public void SetupSliderProperties(UnityEngine.UI.Slider slider)
        {
            slider.maxValue = 1;
            slider.minValue = 0.0001f;
            slider.value = Mathf.Pow(10, currentVolume/20);
            slider.onValueChanged.AddListener(UpdateVolume);
        }

        public void SetupMuteProperties(UnityEngine.UI.Toggle toggle)
        {
            toggle.isOn = isMuted;
            toggle.onValueChanged.AddListener(UpdateMute);
        }
    }
}