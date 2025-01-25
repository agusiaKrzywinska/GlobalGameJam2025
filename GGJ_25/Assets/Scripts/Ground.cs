using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IsGrounded.Add(this);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IsGrounded.Remove(this);
        }
    }
}
