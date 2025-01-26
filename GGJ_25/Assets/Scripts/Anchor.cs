using ANT.Audio;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    [SerializeField]
    private SFXPlayer player;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.Play();
    }
}
