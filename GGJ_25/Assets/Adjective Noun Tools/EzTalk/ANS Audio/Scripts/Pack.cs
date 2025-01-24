using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Android;

namespace ANT.Audio
{
    [CreateAssetMenu(fileName = "New Sound Pack", menuName = "Adjective Noun Tools/Audio/Sound Pack")]
    public class Pack : SoundDataHolder
    {
        [Tooltip("Contains all sounds for one type")]
        public Clip[] sounds;

        [SerializeField]
        private bool isLooping = false;
        public bool IsLooping => isLooping;

        [SerializeField]
        private bool playSameSoundInRow = false;

        [SerializeField, MinMaxSlider(0f, 5f)]
        private Vector2 delayBetweenSounds = Vector2.one;
        public float TimeDelayBetweenSounds => Random.Range(delayBetweenSounds.x, delayBetweenSounds.y);

        private int currentPlayingClip = 0;
        public int CurrentPlayingClip => currentPlayingClip;


        public Clip GetClip()
        {
            if (sounds == null || sounds.Length == 0)
            {
                Debug.LogError($"Sound Pack {name} is missing sounds");
                return null;
            }
            int randomPos;
            Clip clip;
            do
            {
                randomPos = Random.Range(0, sounds.Length);
                clip = sounds[randomPos];
            }
            while (playSameSoundInRow == false && currentPlayingClip == randomPos);

            currentPlayingClip = randomPos;
            if (clip == null)
            {
                Debug.LogError($"Sound Pack {name} has missing clip at {randomPos}");
            }
            return clip;
        }
    }
}
