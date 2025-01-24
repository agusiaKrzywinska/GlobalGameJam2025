using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace ANT.Audio
{
    [RequireComponent(typeof(AudioSource)), DefaultExecutionOrder(-1)]
    public class MusicPlayer : Singleton<MusicPlayer>
    {
        private AudioSource primaryChannel = null;
        [SerializeField]
        private Volume musicVolume;

        [Header("Multi Channel Support")]
        [SerializeField]
        private Volume mainChannelVolume = null;
        [SerializeField]
        private Volume secondaryChannelVolume = null;
        [SerializeField]
        private AudioSource secondaryChannel = null;

        private MultiChannelClip newMultiClip = null;

        private MultiChannelClip currentPlayingClip = null;

        private bool isFadingMusic = false;

        private IEnumerator mainChannelFade;
        private IEnumerator secondaryChannelFade;

        protected override void OnSetup()
        {
            primaryChannel = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundDataHolder sound)
        {
            MultiChannelClip multiChannelClip = sound as MultiChannelClip;
            if(multiChannelClip)
            {
                PlayMultiChannelClip(multiChannelClip);
                return;
            }

            Pack pack = sound as Pack;
            if(pack)
            {
                PlayTrackPack(pack);
                return;
            }
            Sound clip = sound as Sound;
            if(clip)
            {
                PlayTrackSound(clip);
                return;
            }

            Debug.LogError("Could not play sound");
        }

        private void PlayTrackPack(Pack pack)
        {
            if (pack == null)
            {
                Debug.LogError($"Missing pack");
                return;
            }
            Clip myClip = pack.GetClip();
            PlayTrackSound(myClip);
        }

        private void PlayTrackSound(Sound sound)
        {
            if (sound == null)
                Debug.LogError($"Missing clip");
            else if(primaryChannel.clip == sound._Clip)
            {
                if (AudioManager.ShowWarnings)
                    Debug.LogWarning($"Already playing track {sound.name}");
            }
            else if(newMultiClip == null)
            {
                //if it's a multi component track reset it's info
                MultiComponentClip clip = sound as MultiComponentClip;
                if (clip)
                {
                    clip.ResetClip();
                }

                MultiChannelClip tempClip = ScriptableObject.CreateInstance<MultiChannelClip>();
                tempClip.mainChannel = sound;
                tempClip.secondaryChannel = sound;
                
                PlayMultiChannelClip(tempClip);
            }
            else if(primaryChannel.clip == null || sound != newMultiClip.mainChannel)
            {
                //if it's a multi component track reset it's info
                MultiComponentClip clip = sound as MultiComponentClip;
                if(clip)
                {
                    clip.ResetClip();
                }

                MultiChannelClip tempClip = ScriptableObject.CreateInstance<MultiChannelClip>();
                tempClip.mainChannel = sound;
                tempClip.secondaryChannel = sound;
               
                PlayMultiChannelClip(tempClip);
            }
        }

        public void PlayNextComponentSegment(MultiComponentClip clip)
        {
            if(currentPlayingClip == null)
            {
                if (AudioManager.ShowWarnings)
                    Debug.LogWarning("Attempting to play next segment when nothing is playing");
                return;
            }

            if(currentPlayingClip.mainChannel == clip || currentPlayingClip.secondaryChannel == clip)
            {
                clip.PlayNextTrack(this, currentPlayingClip.mainChannel == clip ? primaryChannel : secondaryChannel);
            }
        }

        private void PlayMultiChannelClip(MultiChannelClip clip)
        {
            if(clip == null)
            {
                Debug.LogError($"Missing clip");
                return;
            }

            if(clip.mainChannel == null)
            {
                Debug.LogError($"Missing main clip", clip);
                return;
            }

            if (clip.secondaryChannel == null)
            {
                Debug.LogError($"Missing secondary clip", clip);
                return;
            }

            if (primaryChannel.clip == clip.mainChannel._Clip && secondaryChannel.clip == clip.secondaryChannel._Clip)
            {
                if (AudioManager.ShowWarnings)
                    Debug.LogWarning($"Already playing track {clip.name}");
                return;
            }

            if(primaryChannel.clip == null)
            {
                PlayAudio(clip);
            }
            else if(clip != newMultiClip)
            {
                if(isFadingMusic)
                {
                    StopAllCoroutines();
                }
                StartCoroutine(TransitionMusicChannel(clip));
            }
        }

        public void Fade(bool isPrimary, bool isFadingIn)
        {
            //if (isFadingMusic) return;
            StartCoroutine(StartFade((isPrimary ? mainChannelVolume : secondaryChannelVolume), isFadingIn));
        }

        private IEnumerator TransitionMusicChannel(MultiChannelClip clip)
        {
            isFadingMusic = true;
            newMultiClip = clip;

            yield return StartCoroutine(StartFade(musicVolume, false));

            PlayAudio(clip);
            
            yield return StartCoroutine(StartFade(musicVolume, true));

            newMultiClip = null;
            isFadingMusic = false;
        }

        private IEnumerator StartFade(Volume myVolume, bool fadeIn)
        {
            if (myVolume.isMuted)
                yield break;

            float currentTime = 0;
            float targetVolume = fadeIn ? myVolume.currentVolume : myVolume.minVolume;

            myVolume.mixer.GetFloat(myVolume.parameterName, out float currentVol);

            if (targetVolume == currentVol)
                yield break;

            float fadeTime = fadeIn ? myVolume.fadeInTime : myVolume.fadeOutTime;
            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                float newVol = Mathf.Lerp(currentVol, targetVolume, currentTime / fadeTime);
                myVolume.mixer.SetFloat(myVolume.parameterName, newVol);
                yield return null;
            }
        }

        private IEnumerator CheckForEndOfSound(Sound sound, AudioSource channel)
        {
            yield return new WaitForSecondsRealtime(sound._Clip.length);

            MultiComponentClip clip = sound as MultiComponentClip;
            if(clip && channel.clip == clip.GetClip)
            {
                clip.PlayNextTrack(this, channel);
            }
        }

        private void PlayAudio(MultiChannelClip clip)
        {
            currentPlayingClip = clip;
            PlaySoundAtChannel(clip.mainChannel, primaryChannel);
            PlaySoundAtChannel(clip.secondaryChannel, secondaryChannel);
            mainChannelVolume.mixer.SetFloat(mainChannelVolume.parameterName, mainChannelVolume.currentVolume);
            secondaryChannelVolume.mixer.SetFloat(secondaryChannelVolume.parameterName, secondaryChannelVolume.minVolume);
        }

        public void PlaySoundAtChannel(Sound sound, AudioSource channel)
        {
            if(channel == primaryChannel)
            {
                currentPlayingClip.mainChannel = sound;
            }
            else if(channel == secondaryChannel)
            {
                currentPlayingClip.secondaryChannel = sound;
            }

            sound.UpdateAudioSource(channel);
            channel.Play();
            if(!sound.IsLooping)
            {
                StartCoroutine(CheckForEndOfSound(sound, channel));
            }
        }


    }
}