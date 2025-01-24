using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ANT.Audio
{
    [RequireComponent(typeof(AudioSource)), DefaultExecutionOrder(-1)]
    public class SFXPlayer : MonoBehaviour
    {
        private SoundDataHolder soundPlaying;
        public SoundDataHolder SoundPlaying => soundPlaying;
        protected AudioSource audioSource = null;
        public AudioSource AudioSource => audioSource;
        [SerializeField]
        private bool changeWhilePlaying = false;
        [SerializeField]
        private bool rePlayActiveSound = false;
        [SerializeField]
        private bool allowQueuingSounds = false;

        [SerializeField]
        private bool pauseWithTimeScale = true;

        private Queue<SoundDataHolder> queuedSounds = new();

        [SerializeField]
        protected SoundDataHolder playOnEnableSfx;
        [SerializeField, ShowIf(nameof(showRandomizeStartTime))]
        private bool playOnStart = true;

        [SerializeField, ShowIf(nameof(showRandomizeStartTime))]
        private bool randomizeStartTime;

        private bool showRandomizeStartTime => playOnEnableSfx != null;

        public UnityAction OnChangeSound;

        private float delayBetweenSounds;

        protected bool isPaused = false;

        public bool IsPlaying => audioSource.isPlaying || queuedSounds.Count > 0;

        public void UpdatePauseState(bool state)
        {
            if (state)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.UnPause();
            }

            isPaused = state;
        }

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.ignoreListenerPause = !pauseWithTimeScale;
        }

        private void OnEnable()
        {
            if (playOnEnableSfx != null)
            {
                if (playOnStart)
                    PlaySound(playOnEnableSfx);
                else
                {
                    SwapSound(playOnEnableSfx);
                    if (playOnEnableSfx is Pack)
                    {
                        isPaused = true;
                    }
                }
            }
        }

        protected virtual void Update()
        {
            UpdateQueuedSounds();

            UpdateSoundPackSounds();
        }

        private void UpdateSoundPackSounds()
        {
            if (soundPlaying is Pack pack)
            {
                if (pack.IsLooping == false) return;

                if (isPaused) return;

                if (audioSource.isPlaying == false)
                {
                    //TODO add check to see if the game is paused to not play a new sound
                    delayBetweenSounds -= Time.deltaTime;
                    if (delayBetweenSounds <= 0f)
                        PlaySoundPack(pack);
                }
            }
        }

        private void UpdateQueuedSounds()
        {
            if (allowQueuingSounds == false) return;

            if (queuedSounds.Count == 0) return;

            if (audioSource.clip == null) return;

            /*if (Mathf.Clamp01(audioSource.time / audioSource.clip.length) == 1f)
            {*/
            if (audioSource.isPlaying == false)
            {
                //trigger next queued song
                SoundDataHolder sfx = queuedSounds.Dequeue();
                PlaySound(sfx);
            }
            //}
        }

        public void PlaySound(SoundDataHolder[] sounds)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                PlaySound(sounds[i]);
            }
        }

        public void PlaySound(SoundDataHolder soundHolder)
        {
            PlaySound(soundHolder, false);
        }

        private void PlaySound(SoundDataHolder soundHolder, bool inPack)
        {
            if (audioSource == null)
                audioSource = GetComponent<AudioSource>();

            Sound sound = soundHolder as Sound;
            if (sound)
            {
                MultiComponentClip multiComponent = soundHolder as MultiComponentClip;
                if (multiComponent)
                {
                    Debug.LogError("Could not play multi component clip in SFX");
                    return;
                }
                Clip clip = sound as Clip;
                if (clip)
                {
                    PlaySoundClip(clip, inPack);
                    return;
                }
            }

            Pack pack = soundHolder as Pack;
            if (pack)
            {
                PlaySoundPack(pack, true);
                return;
            }

            MultiChannelClip multiChannel = soundHolder as MultiChannelClip;
            if (multiChannel)
            {
                Debug.LogError("Could not play multi channel clip in SFX");
                return;
            }


            if (soundHolder == null)
            {
                Debug.LogError("Missing Audio!");
            }
            Debug.LogError("Could not play sound");
        }

        private bool CanPlay()
        {
            if(audioSource == null) return false;

            if (audioSource.isPlaying && rePlayActiveSound == false)
                return false;

            if (audioSource.clip == null)
                return false;

            return true;
        }

        public void Play()
        {
            if (CanPlay())
            {
                if (randomizeStartTime)
                {
                    audioSource.time = Random.value * audioSource.clip.length;
                }
                audioSource.Play();
                if (isPaused)
                {
                    isPaused = false;
                }
            }
        }

        public void Stop()
        {
            if (audioSource.clip)
            {
                audioSource.Stop();
                if (isPaused == false)
                {
                    isPaused = true;
                }
            }
        }

        private void PlaySoundPack(Pack soundPack)
        {
            PlaySoundPack(soundPack, true);
        }

        private void PlaySoundPack(Pack soundPack, bool inPack)
        {
            if (soundPack == null)
            {
                Debug.LogError($"Missing sound pack");
                return;
            }

            if (allowQueuingSounds && audioSource.isPlaying && audioSource.clip != null)
            {
                Debug.Log($"queued sound pack {soundPack.name} on {name}", this);
                queuedSounds.Enqueue(soundPack);
                return;
            }

            if (audioSource.isPlaying && !changeWhilePlaying)
            {
                Debug.Log($"Audio source is still playing so will not play from {soundPack.name} pack", gameObject);
                return;
            }

            soundPlaying = soundPack;
            delayBetweenSounds = soundPack.TimeDelayBetweenSounds;
            Clip myClip = soundPack.GetClip();
            PlaySound(myClip, inPack);
        }

        private void PlaySoundClip(Clip clip, bool inPack)
        {
            SwapClip(clip, inPack);
            Play();
        }

        public void SwapSound(SoundDataHolder sound)
        {
            SwapSound(sound, false);
        }
        public void SwapSound(SoundDataHolder sound, bool inPack)
        {
            Pack pack = sound as Pack;
            if (pack)
            {
                SwapPack(pack, true);
                return;
            }
            Clip clip = sound as Clip;
            if (clip)
            {
                SwapClip(clip, inPack);
                return;
            }

            Debug.LogError("Could not swap sound");
        }

        private void SwapPack(Pack soundPack, bool inPack)
        {
            if (soundPack == null)
            {
                Debug.LogError($"Missing sound pack");
                return;
            }

            if (audioSource.isPlaying && !changeWhilePlaying)
            {
                Debug.Log($"Audio source is still playing so will not play from {soundPack.name} pack", gameObject);
                return;
            }

            soundPlaying = soundPack;
            Clip myClip = soundPack.GetClip();
            SwapClip(myClip, true);
        }

        private void SwapClip(Clip clip, bool inPack)
        {
            if (clip == null)
            {
                Debug.LogError($"Missing clip");
                return;
            }

            if (allowQueuingSounds && audioSource.isPlaying && audioSource.clip != null)
            {
                Debug.Log($"queued clip {clip.name} on {name}", this);
                queuedSounds.Enqueue(clip);
                return;
            }

            if (audioSource.clip == clip.clip && !rePlayActiveSound)
            {
                if (AudioManager.ShowWarnings)
                    Debug.LogWarning($"Audio source is still playing {clip.name} clip", gameObject);
                return;
            }

            if (audioSource.isPlaying && !changeWhilePlaying)
            {
                if (AudioManager.ShowWarnings)
                    Debug.LogWarning($"Audio source is still playing so will not play {clip.name} clip", gameObject);
                return;
            }

            clip.UpdateAudioSource(audioSource);

            if (inPack == false)
                soundPlaying = clip;
        }

        public void ClearAudioQueue()
        {
            if (allowQueuingSounds)
            {
                queuedSounds.Clear();
                Stop();
            }
        }

    }
}