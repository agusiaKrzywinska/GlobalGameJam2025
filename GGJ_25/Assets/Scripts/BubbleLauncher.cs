using ANT;
using UnityEngine;

public class BubbleLauncher : MonoBehaviour
{
    public BubbleController bubbleController;
    public float growthRate = 1f;

    public bool increaseBubbleSize = true;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera panningCamera;

    [SerializeField]
    private Cinemachine.CinemachineBrain brain;

    bool startDelay = true;

    // Start is called before the first frame update
    void Start()
    {
        this.DelayedExecute(3f, () => startDelay = false);
    }

    // Update is called once per frame
    void Update()
    {
        if (startDelay) return;
        if (bubbleController.isInLauncher == false) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            panningCamera.enabled = !panningCamera.enabled;
        }

        if (panningCamera.enabled || brain.IsBlending)
        {
            return;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            bubbleController.IncreaseBubbleSize((increaseBubbleSize ? 1 : -1) * growthRate * Time.deltaTime);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            bubbleController.LaunchBubble();
            bubbleController.isInLauncher = false;
        }
    }
}
