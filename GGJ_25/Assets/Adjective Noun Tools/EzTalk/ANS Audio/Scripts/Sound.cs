using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{
    public abstract class Sound : SoundDataHolder
    {
        public virtual bool IsLooping { get; }

        public virtual AudioClip _Clip { get; }
        public virtual void UpdateAudioSource(AudioSource source)
        {
            source.loop = IsLooping;
        }
    }
}