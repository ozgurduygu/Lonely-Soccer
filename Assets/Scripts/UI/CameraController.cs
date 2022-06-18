using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera topCamera, ballCamera;
    [SerializeField] private CinemachineBrain cinemachineBrain;

    public enum Camera
    {
        top,
        ball
    }

    public float CameraBlendDuration 
    {
        get => cinemachineBrain.m_DefaultBlend.BlendTime;
    }

public void SetCamera(Camera camera)
{
    if (camera == Camera.ball)
    {
        ballCamera.Priority = topCamera.Priority + 1;
    }
    else
    {
        ballCamera.Priority = topCamera.Priority - 1;
    }
}
}
