using ANT.Audio;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace ANT.EzTalk
{
    [CreateAssetMenu(fileName = "New Speaker", menuName = "Adjective Noun Tools/EzTalk/Speaker")]
    public class Speaker : ScriptableObject
    {
        [SerializeField]
        private string _name;
        public string Name => _name;
        [SerializeField]
        private string speakerKey;
        public string SpeakerKey => speakerKey;

        [SerializeField, OnValueChanged("UpdateDefaultSpeakerImage")]
        private Sprite speakerPicture;
        [SerializeField, ReadOnly]
        private Sprite defaultSpeakerPicture;
        public Sprite SpeakerPicture => speakerPicture;
        [SerializeField]
        private Sound voice;
        [SerializeField]
        private bool overrideReverb;
        public bool OverrideReverb => overrideReverb;
        [SerializeField, ShowIf(nameof(overrideReverb))]
        private float reverb;
        public float Reverb => reverb;

        public UnityAction OnUpdatedSpeaker;

        private void UpdateDefaultSpeakerImage()
        {
            defaultSpeakerPicture = speakerPicture;
        }

        private void OnEnable()
        {
            speakerPicture = defaultSpeakerPicture;
        }

        public void PlayVoice()
        {
            if (voice)
                EzTalkCanvas.Player.PlaySound(voice);
        }

        public void ChangeSpeakerImage(Sprite newSprite)
        {
            speakerPicture = newSprite;
            OnUpdatedSpeaker?.Invoke();
        }
    }
}