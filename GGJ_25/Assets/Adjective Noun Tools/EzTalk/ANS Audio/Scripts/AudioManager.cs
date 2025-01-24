using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace ANT.Audio
{

    [DefaultExecutionOrder(-10)]
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField]
        private bool showLogs = true;
        [SerializeField]
        private bool showWarnings = true;

        public static bool ShowWarnings => Instance.showWarnings;
        public static bool ShowLogs => Instance.showLogs;

        [SerializeField]
        private SnapShotInfo[] snapshots;

        [SerializeField]
        private SFXPlayer globalAudioSource;

        [SerializeField]
        private AudioMixer globalAudio;
        [SerializeField]
        private float[] soundBands;
        public static int MaxSoundBands => Instance.soundBands.Length;
        private SNAPSHOT_TYPE currentType;
        public SNAPSHOT_TYPE CurrentType => currentType;

        [SerializeField]
        private int soundVolume = 0;
        public static int SoundVolume { get => Instance.soundVolume; set => Instance.soundVolume = value; }

        private const string VOL = "Vol";

        public void PlayGlobalSound(Clip clip)
        {
            globalAudioSource.PlaySound(clip);
        }

        public void UpdateVolume()
        {
            globalAudio.SetFloat(VOL, Mathf.Log10(soundBands[SoundVolume]) * 20f);
        }

        public void TransitionSnapshot(SNAPSHOT_TYPE type)
        {
            if (currentType == type) return;

            SnapShotInfo snapshotInfo = snapshots.First(x => x.type == type);

            snapshotInfo.snapshot.TransitionTo(snapshotInfo.transitionTime);

            currentType = type;
        }

        public void FadeAudio(bool fadeIn)
        {
            StartCoroutine(StartFade(fadeIn));
        }

        public void MuteAudio()
        {
            globalAudio.SetFloat(VOL, -80f);
        }

        private IEnumerator StartFade(bool fadeIn)
        {
            float currentTime = 0;
            float targetVolume = fadeIn ? Mathf.Log10(soundBands[SoundVolume]) * 20f : -80f;

            globalAudio.GetFloat(VOL, out float currentVol);

            if (targetVolume == currentVol)
                yield break;

            float fadeTime = fadeIn ? 1f : 1f;
            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / fadeTime);
                globalAudio.SetFloat(VOL, newVol);
                yield return null;
            }
        }
    }

    public enum SNAPSHOT_TYPE
    {
        DEFAULT,
        FISHING,
        MOLE_VISION,
        INTERACTION,
        MOLE_VISION_INTERACTION,
    }

    [System.Serializable]
    public class SnapShotInfo
    {
        public SNAPSHOT_TYPE type;
        public float transitionTime = 0.1f;
        public AudioMixerSnapshot snapshot;
    }
}