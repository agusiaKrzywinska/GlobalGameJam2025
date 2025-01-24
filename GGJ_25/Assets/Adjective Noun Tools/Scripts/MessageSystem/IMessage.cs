using System.Collections;
using System.Collections.Generic;
using ANT.Audio;
using UnityEngine.Events;


namespace ANT
{
    public interface IMessage
    {
        /// <summary>
        /// the text that is the message. 
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// The audio which the text matches. 
        /// </summary>
        public SoundDataHolder[] Audio { get; }

        /// <summary>
        /// what to set the reverb to when it reads this message. 
        /// </summary>
        public float ReverbAmount { get; }

        /// <summary>
        /// An action that fires once the message has played. 
        /// </summary>
        public UnityAction OnComplete { get; }
    }
}