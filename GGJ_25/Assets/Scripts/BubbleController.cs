using NaughtyAttributes;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [MinMaxSlider(0f, 10f)]
    public Vector2 bubbleSize;

    public bool isInLauncher = true;

    public BubbleLauncher launcher;
    [SerializeField]
    private Vector3 floatUpSpeed = Vector3.up;

    [SerializeField]
    private float xSpeed = 1f;

    [SerializeField]
    private float launchForce = 10f;

    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInLauncher) return;

        body.AddForce(floatUpSpeed);
        //apply upwards movement. 
        //check for left and right movement. 
        float direction = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        body.AddForce(direction * xSpeed * Vector2.right);
    }

    public void IncreaseBubbleSize(float amount)
    {
        float currentSize = transform.localScale.x + amount;
        if (currentSize > bubbleSize.y)
        {
            currentSize = bubbleSize.y;
            launcher.increaseBubbleSize = false;

        }
        else if (currentSize < bubbleSize.x)
        {
            currentSize = bubbleSize.x;
            launcher.increaseBubbleSize = true;
        }

        transform.localScale = Vector3.one * currentSize;

        body.mass = currentSize;
    }

    public void LaunchBubble()
    {
        body.AddForce(Vector2.right * launchForce);
    }
}
