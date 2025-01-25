using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    private float spinSpeed;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}
