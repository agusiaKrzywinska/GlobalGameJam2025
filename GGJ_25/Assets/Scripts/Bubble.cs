using UnityEngine;

public class Bubble : MonoBehaviour
{
    private void Awake()
    {
        LevelManager.Instance.bubblesInLevel.Add(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out BubbleController bubble))
        {
            bubble.IncreaseBubbleSize(transform.localScale.x);
            gameObject.SetActive(false);
        }

    }
}
