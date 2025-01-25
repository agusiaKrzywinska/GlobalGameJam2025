using UnityEngine;
using UnityEngine.Events;

public class EventOnTrigger : MonoBehaviour
{
    public UnityEvent OnTrigger;
    public UnityEvent OnTriggerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<BubbleController>())
        {
            OnTrigger.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<BubbleController>())
        {
            OnTrigger.Invoke();
        }
    }
}
