using UnityEngine;

public class ForceZone : MonoBehaviour
{
    [SerializeField]
    private Vector2 force;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.body.AddForce(force);
        }
    }
}
