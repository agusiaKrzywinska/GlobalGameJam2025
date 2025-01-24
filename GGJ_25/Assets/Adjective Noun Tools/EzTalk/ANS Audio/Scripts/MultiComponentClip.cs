using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{
    public class MultiComponentClip : Sound
    {
        [SerializeField]
        private Clip[] components;

        private int currentPlaceInTrack = 0;

        public override bool IsLooping => GetClip.IsLooping;
        public override AudioClip _Clip => GetClip._Clip; 

        public override void UpdateAudioSource(AudioSource source)
        {
            GetClip.UpdateAudioSource(source);
        }

        public void ResetClip()
        {
            currentPlaceInTrack = 0;
        }

        public Sound GetClip => components[currentPlaceInTrack];

        public void PlayNextTrack(MusicPlayer player, AudioSource source)
        {
            currentPlaceInTrack++;
            if (currentPlaceInTrack == components.Length)
                return;

            player.PlaySoundAtChannel(this, source);
        }
    }
}