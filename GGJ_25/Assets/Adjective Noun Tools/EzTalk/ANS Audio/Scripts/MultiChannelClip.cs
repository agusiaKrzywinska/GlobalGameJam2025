using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ANT.Audio
{
    [CreateAssetMenu(fileName = "New Audio Clip", menuName = "Adjective Noun Tools/Audio/Mutlichannel Audio Clip")]
    public class MultiChannelClip : SoundDataHolder
    {
        public Sound mainChannel;
        public Sound secondaryChannel;
    }
}