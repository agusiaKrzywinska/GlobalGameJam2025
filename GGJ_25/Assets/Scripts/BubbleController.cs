using ANT;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BubbleController : MonoBehaviour
{
    [MinMaxSlider(0f, 10f)]
    public Vector2 bubbleSize;

    [HideInInspector]
    public bool isInLauncher = true;
    [HideInInspector]
    public bool isInEndZone = false;
    [HideInInspector]
    public bool goNext = false;

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
    [HideInInspector]
    public Rigidbody2D body;

    [SerializeField]
    private float velocityCap = 10f;

    [SerializeField]
    private float sfxThresholdSpeed = 5f;

    private float frozenTimeLeft = 0f;
    public bool IsFrozen => frozenTimeLeft > 0f;

    public List<Ground> IsGrounded = new List<Ground>();

    [SerializeField]
    private float offsetSizing = 7f;

    [SerializeField]
    private UnityEvent onPop;

    [SerializeField]
    private ParticleSystem hitParticle;

    public BubbleSFXManager sfx;

    [SerializeField]
    private Image freezeTimer;

    private float maxFreezeTime = 1f;

    [SerializeField]
    private ParticleSystem popEffect;
    [SerializeField]
    private GameObject visrep;

    [SerializeField]
    private ParticleSystem trailEffect;

    // Start is called before the first frame update
    void Awake()
    {
        startPosition = transform.position;
        body = GetComponent<Rigidbody2D>();

        sfx = GetComponent<BubbleSFXManager>();

        body.mass = bubbleSize.x;
        transform.localScale = Vector3.one * bubbleSize.x;

        LevelManager.Instance.mainBubble = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (visrep.activeInHierarchy == false)
        {
            body.velocity = Vector2.zero;
            sfx.StopSFX(BubbleSFXManager.SoundType.Bubble_Move);
            return;
        }
        if (goNext && Input.GetKeyDown(KeyCode.RightArrow))
        {
            LoadingManager.Instance.LoadScene(LevelManager.Instance.nextLevel);
        }

        if (isInLauncher)
        {
            body.velocity = Vector2.zero;
            sfx.StopSFX(BubbleSFXManager.SoundType.Bubble_Move);
            return;
        }

        if (isInEndZone)
        {
            sfx.StopSFX(BubbleSFXManager.SoundType.Bubble_Move);
            return;
        }

        //setup to sink if frozen. 
        body.gravityScale = IsFrozen ? 1f : 0f;
        if (IsFrozen)
        {
            frozenTimeLeft -= Time.deltaTime;
        }
        else
        {
            //apply upwards movement. 
            transform.position += floatUpSpeed * Time.deltaTime * (bubbleSize.y - transform.localScale.x);
        }
        //check for left and right movement. 
        float direction = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) + (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0);

        if (IsFrozen == false || IsGrounded.Count > 0)
        {
            //apply movement 
            body.AddForce(direction * xSpeed * Vector2.right);
        }

        if (IsFrozen)
        {
            trailEffect.Stop();
        }
        else if (trailEffect.isEmitting == false)
        {
            trailEffect.Play();
        }

        //cap velocity if needed
        if (body.velocity.magnitude > velocityCap)
        {
            body.velocity = body.velocity.normalized * velocityCap;
        }

        if (body.velocity.magnitude >= sfxThresholdSpeed && (IsFrozen == false))
        {
            sfx.PlaySFX(BubbleSFXManager.SoundType.Bubble_Move);
        }
        else
        {
            sfx.StopSFX(BubbleSFXManager.SoundType.Bubble_Move);
        }


        freezeTimer.fillAmount = frozenTimeLeft / maxFreezeTime;


        //only shrink if not frozen.
        if (IsFrozen == false)
        {
            IncreaseBubbleSize(-sizeDecreaseRate * Time.deltaTime);
        }
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

            //find the differece of the scale. 
            amount = currentSize - transform.localScale.x;
            transform.position += new Vector3(amount, amount) * offsetSizing;
        }
        else if (currentSize > bubbleSize.y || currentSize < bubbleSize.x)
        {
            Pop();
            return;
        }

        transform.localScale = Vector3.one * currentSize;
        body.mass = currentSize;
    }

    public void FreezeBubble(float timeFrozen)
    {
        maxFreezeTime = timeFrozen;
        sfx.PlaySFX(BubbleSFXManager.SoundType.Bubble_Freeze);
        frozenTimeLeft = timeFrozen;
    }

    public void LaunchBubble()
    {
        body.AddForce(Vector2.right * launchForce);
    }

    public void Pop()
    {
        if (popEffect.isEmitting) return;
        //todo show explosion effect. 
        visrep.SetActive(false);
        popEffect.Play();
        sfx.PlaySFX(BubbleSFXManager.SoundType.Bubble_Pop);
        this.DelayedExecute(popEffect.main.duration, () =>
        {
            visrep.SetActive(true);
            isInLauncher = true;
            onPop.Invoke();
            body.mass = bubbleSize.x;
            transform.localScale = Vector3.one * bubbleSize.x;
            transform.position = startPosition;
            LevelManager.Instance.ResetLevel();
            IsGrounded.Clear();
        });
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (body.velocity.magnitude >= sfxThresholdSpeed)
        {
            if (IsFrozen)
            {
                sfx.PlaySFX(BubbleSFXManager.SoundType.Bubble_Frozen_Impact);
            }
            else
            {
                sfx.PlaySFX(BubbleSFXManager.SoundType.Bubble_Impact);
                //spawn hit Ps at collision point
                Destroy(Instantiate(hitParticle, collision.contacts[0].point, Quaternion.Euler((Vector3)(collision.contacts[0].point) - transform.position)), hitParticle.main.duration);
            }
        }
    }
}
