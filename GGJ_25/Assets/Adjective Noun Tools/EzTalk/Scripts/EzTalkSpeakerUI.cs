using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace ANT.EzTalk
{
    public class EzTalkSpeakerUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject parentUI;
        [SerializeField]
        private Image _profile = null;
        [SerializeField]
        private bool showProfileAlways = true;
        [SerializeField]
        private EzTalkText _name = null;
        [SerializeField]
        private bool showNameAlways = true;
        [SerializeField]
        private EzTalkText _message = null;

        [SerializeField]
        private bool showMessageAlways = true;
        [NaughtyAttributes.ShowNonSerializedField]
        private Speaker speaker;

        private SFXPlayer sfxPlayer;
        public SFXPlayer SFXPlayer => sfxPlayer;

        private float defaultReverb;

        private void Awake()
        {
            sfxPlayer = GetComponent<SFXPlayer>();

            _name.SetupTypewriter();
            _message.SetupTypewriter();
        }

        public void Start()
        {
            defaultReverb = sfxPlayer.AudioSource.reverbZoneMix;
        }

        public void UpdateSpeakerUI(Speaker speaker)
        {
            if (this.speaker)
                this.speaker.OnUpdatedSpeaker -= UpdateSpeakerUI;
            this.speaker = speaker;
            if (this.speaker)
                this.speaker.OnUpdatedSpeaker += UpdateSpeakerUI;
            UpdateSpeakerUI();
        }

        private void UpdateSpeakerUI()
        {
            if (speaker == null)
            {
                if (_profile)
                    _profile.sprite = null;
                _name.Write("");
                return;
            }

            if (_profile && speaker.SpeakerPicture)
                _profile.sprite = speaker.SpeakerPicture;

            _name.Write(speaker.Name);

            sfxPlayer.AudioSource.reverbZoneMix = speaker.OverrideReverb ? speaker.Reverb : defaultReverb;
        }

        public void SetUIState(bool state)
        {
            parentUI.SetActive(state);
            _profile.gameObject.SetActive(state);
            _message.SetActive(state);
            _name.SetActive(state);
        }


        public void UpdateUI(bool state)
        {
            parentUI.SetActive(state);
            _profile.gameObject.SetActive(state || showProfileAlways);
            _message.SetActive(state || showMessageAlways);
            _name.SetActive(state || showNameAlways);
        }

        public void UpdateMessage(string message)
        {
            _message.Write(message);
        }


        public bool HasTypewriterToSkip()
        {
            return _message.CanSkipTyping();
        }

        public void SkipTypeWriter()
        {
            _message.SkipTyping();
        }
    }
}