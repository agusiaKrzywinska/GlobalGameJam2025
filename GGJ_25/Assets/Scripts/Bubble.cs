using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private float sizeMultiplier = 0.25f;

    [SerializeField]
    private Vector3 floatUpSpeed = Vector3.zero;
    [SerializeField]
    public Vector3 startPosition;

    [HideInInspector]
    public bool isSpawned;
    private void Awake()
    {
        LevelManager.Instance.bubblesInLevel.Add(this);
        startPosition = transform.position;
    }

    private void OnDestroy()
    {
        LevelManager.Instance?.bubblesInLevel.Remove(this);
    }

    private void Update()
    {
        if (LevelManager.Instance.mainBubble.isInLauncher || LevelManager.Instance.mainBubble.isInEndZone) return;

        transform.position += floatUpSpeed * Time.deltaTime * (LevelManager.Instance.mainBubble.bubbleSize.y - transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IncreaseBubbleSize(transform.localScale.x * sizeMultiplier);
            gameObject.SetActive(false);
        }
    }
}
