using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ANT.EzTalk
{
    public class DialogueSpeakerSwitcher : MonoBehaviour
    {
        public EzTalk.Dialogue dialogue;
        public Animator speaker1Anim;
        public Sprite speaker1PFP;
        public string speaker1Name;
        public Image PFP1;
        public TMPro.TextMeshProUGUI Name1;
        public Animator speaker2Anim;
        public Sprite speaker2PFP;
        public string speaker2Name;
        public Image PFP2;
        public TMPro.TextMeshProUGUI Name2;
        public bool currentState;

        public void Start()
        {
            Name1.text = speaker1Name;
            Name2.text = speaker2Name;
            PFP1.sprite = speaker1PFP;
            PFP2.sprite = speaker2PFP;
            if (currentState)
            {
                Name2.gameObject.SetActive(false);
                speaker2Anim.SetBool("Dim", true);
            }
            else
            {
                Name1.gameObject.SetActive(false);
                speaker1Anim.SetBool("Dim", true);
            }
        }

        public void ChangeSpeaker()
        {
            if(currentState)
            {
                Name1.gameObject.SetActive(false);
                Name2.gameObject.SetActive(true);
                speaker1Anim.SetBool("Dim",true);
                speaker2Anim.SetBool("Dim", false);
                currentState = !currentState;
            }
            else
            {
                Name2.gameObject.SetActive(false);
                Name1.gameObject.SetActive(true);
                speaker2Anim.SetBool("Dim", true);
                speaker1Anim.SetBool("Dim", false);
                currentState = !currentState;
            }
        }

    }
}
