using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]
    private float sizeMultiplier = 0.25f;
    private void Awake()
    {
        LevelManager.Instance.bubblesInLevel.Add(this);
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
