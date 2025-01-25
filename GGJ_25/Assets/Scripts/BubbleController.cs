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

    [SerializeField]
    private float sizeDecreaseRate = 0.01f;
    private Vector3 startPosition;
    private Rigidbody2D body;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        body = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isInLauncher)
        {
            body.velocity = Vector2.zero;
            return;
        }
        //apply upwards movement. 
        body.AddForce(floatUpSpeed);
        //check for left and right movement. 
        float direction = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);
        body.AddForce(direction * xSpeed * Vector2.right);

        IncreaseBubbleSize(-sizeDecreaseRate * Time.deltaTime);
    }

    public void IncreaseBubbleSize(float amount)
    {
        float currentSize = transform.localScale.x + amount;
        if (isInLauncher)
        {
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
        }
        else if (currentSize > bubbleSize.y || currentSize < bubbleSize.x)
        {
            Pop();
            return;
        }

        transform.localScale = Vector3.one * currentSize;
        body.mass = currentSize;
    }

    public void LaunchBubble()
    {
        body.AddForce(Vector2.right * launchForce);
    }

    public void Pop()
    {
        //todo show explosion effect. 
        isInLauncher = true;
        body.mass = bubbleSize.x;
        transform.localScale = Vector3.one * bubbleSize.x;
        transform.position = startPosition;

    }
}
