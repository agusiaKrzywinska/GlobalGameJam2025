using ANT.ScriptableProperties;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{
    [CreateAssetMenu(fileName = "New Audio Clip", menuName = "Adjective Noun Tools/Audio/Audio Clip")]
    public class Clip : Sound
    {
        public AudioClip clip;
        public override AudioClip _Clip => clip;
        public bool isLooping = false;
        public override bool IsLooping => isLooping;
        
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(-3f, 3f)]
        public float lowPitch = 1f;
        [Range(-3f, 3f)]
        public float highPitch = 1f;

        [HideInInspector]
        public override void UpdateAudioSource(AudioSource source)
        {
            source.clip = clip;
            source.loop = isLooping;
            source.volume = volume;
            source.pitch = Random.Range(lowPitch, highPitch);
        }
        public void PlaySoundGlobally()
        {
            AudioManager.Instance.PlayGlobalSound(this);
        }
    }
}
