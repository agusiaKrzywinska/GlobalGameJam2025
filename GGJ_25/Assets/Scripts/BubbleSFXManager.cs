using ANT.Audio;
using System.Linq;
using UnityEngine;

public class BubbleSFXManager : MonoBehaviour
{

    [SerializeField]
    private SFX[] soundEffects;

    public void PlaySFX(SoundType type)
    {
        var player = soundEffects.FirstOrDefault(x => x.type == type).player;
        player.Play();
    }

    [System.Serializable]
    public class SFX
    {
        public SoundType type;
        public SFXPlayer player;
    }
    public enum SoundType
    {
        Bubble_Impact,
        Bubble_Move,
        Bubble_Pop,
        Bubble_Pickup,
        Bubble_Freeze,
        Bubble_Frozen_Impact
    }

}
