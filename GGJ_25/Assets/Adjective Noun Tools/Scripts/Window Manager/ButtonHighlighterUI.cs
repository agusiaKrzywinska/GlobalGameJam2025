using ANT.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ANT
{
    [RequireComponent(typeof(Button))]
    public class ButtonHighlighterUI : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField, Tooltip("When selected this will play these SFX.")]
        private Clip[] sfxOnHighlight;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        /// <summary>
        /// Change the sfx clip to be what's passed through. 
        /// </summary>
        /// <param name="clip"></param>
        public void ChangeClip(Clip clip)
        {
            sfxOnHighlight = new Clip[] {clip};
        }

        /// <summary>
        /// When you select the this button play the message. 
        /// </summary>
        /// <param name="eventData">event system information that gets passed through from the OnSelect method</param>
        public void OnSelect(BaseEventData eventData)
        {
            if (WindowManager.ClearQueue)
            {
                WindowManager.Player.ClearAudioQueue();
            }
            PlayButtonMessage();
            ANT.Audio.AudioManager.Instance.PlayGlobalSound(WindowManager.UiSelectionSFX);
        }

        /// <summary>
        /// play all the sounds from the windows manager. 
        /// </summary>
        public void PlayButtonMessage()
        {
            WindowManager.Player.PlaySound(sfxOnHighlight);
        }

        /// <summary>
        /// if you mouse over this button play the audio. 
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (button.interactable == false || button.enabled == false) return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}