using UnityEngine;

public class FrozenSpriteSwapper : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite defaultSprite;
    [SerializeField]
    private Sprite frozenSprite;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sprite = LevelManager.Instance.mainBubble.IsFrozen ? frozenSprite : defaultSprite;
    }
}
