using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class EndGoal : MonoBehaviour
{
    [SerializeField]
    private float pullRange;
    [SerializeField]
    private float pullForce;
    [SerializeField]
    private UnityEvent onCompleteMovement;
    [SerializeField]
    private CinemachineVirtualCamera endCam;
    private bool completedMovement;
    private bool firedEvent;
    [SerializeField]
    private Transform sealPoint;

    private BubbleLauncher launcher;
    private void Start()
    {
        launcher = FindAnyObjectByType<BubbleLauncher>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.isInEndZone = true;
        }
    }

    private void Update()
    {
        if (LevelManager.Instance.mainBubble.isInEndZone && completedMovement == false)
        {
            LevelManager.Instance.mainBubble.body.velocity = Vector3.zero;
            LevelManager.Instance.mainBubble.transform.position = Vector3.MoveTowards(LevelManager.Instance.mainBubble.transform.position, transform.position, pullForce);
            if (LevelManager.Instance.mainBubble.transform.position == transform.position)
            {
                launcher.transform.position = sealPoint.position;
                endCam.enabled = true;
                completedMovement = true;
            }
        }
        else if (completedMovement == true && LevelManager.Instance.brain.IsBlending == false && firedEvent == false)
        {
            firedEvent = true;
            onCompleteMovement.Invoke();
        }
        else if (completedMovement == false)
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
