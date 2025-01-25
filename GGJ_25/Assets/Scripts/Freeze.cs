using UnityEngine;

public class Freeze : MonoBehaviour
{
    [SerializeField]
    private float freezeTime;
    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.FreezeBubble(freezeTime);
        }
    }
}
