using UnityEngine;

public class EyeFollow : MonoBehaviour
{
    public Transform Pupil;
    public Transform Player;
    public float EyeRadius = 1f;
    Vector3 mPupilCenterPos;

    void Start()
    {
        mPupilCenterPos = Pupil.position;
    }

    void Update()
    {
        Vector3 lookDir = (Player.position - mPupilCenterPos).normalized;
        Pupil.position = mPupilCenterPos + (lookDir * EyeRadius);
    }
}
