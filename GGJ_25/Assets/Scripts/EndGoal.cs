using UnityEngine;

public class EndGoal : MonoBehaviour
{
    [SerializeField]
    private float pullRange;
    [SerializeField]
    private float pullForce;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            //TODO end level. 
            bubble.isInEndZone = true;

        }
    }

    private void Update()
    {
        if (LevelManager.Instance.mainBubble.isInEndZone)
        {
            LevelManager.Instance.mainBubble.body.velocity = Vector3.zero;
            LevelManager.Instance.mainBubble.transform.position = Vector3.MoveTowards(LevelManager.Instance.mainBubble.transform.position, transform.position, pullForce);
        }
        else
        {
            Vector3 bubblePos = LevelManager.Instance.mainBubble.transform.position;
            float distanceBetween = Vector3.Distance(bubblePos, transform.position);

            if (distanceBetween < pullRange)
            {
                LevelManager.Instance.mainBubble.body.AddForce((transform.position - bubblePos).normalized * pullForce);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pullRange);
    }
}
