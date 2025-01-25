using UnityEngine;

public class Ground : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IsGrounded = true;
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
