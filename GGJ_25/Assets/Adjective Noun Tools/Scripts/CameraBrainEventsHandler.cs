using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CinemachineBrain))]
public class CameraBrainEventsHandler : MonoBehaviour
{
    public event UnityAction<ICinemachineCamera> OnBlendStarted;
    public event UnityAction<ICinemachineCamera> OnBlendFinished;

    CinemachineBrain mBrain;
    Coroutine _trackingBlend;
    // Start is called before the first frame update
    void Start()
    {
        mBrain = GetComponent<CinemachineBrain>();
        mBrain.m_CameraActivatedEvent.AddListener(OnCameraActivated);
    }

    void OnCameraActivated(ICinemachineCamera newCamera, ICinemachineCamera previousCamera)
    {
        IEnumerator WaitForBlendCompleition()
        {
            while (mBrain.IsBlending)
            {
                yield return null;
            }

            OnBlendFinished.Invoke(newCamera);
            _trackingBlend = null;
        }

        if (_trackingBlend != null)
        {
            StopCoroutine(_trackingBlend);

            OnBlendStarted.Invoke(previousCamera);
            _trackingBlend = StartCoroutine(WaitForBlendCompleition());

        }
    }
}
