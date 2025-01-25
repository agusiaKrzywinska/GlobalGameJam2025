using UnityEngine;

public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            if (bubble.IsFrozen == false)
            {
                bubble.Pop();
            }
            else
            {
                bubble.IsGrounded = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IsGrounded = false;
        }
    }
}