using Cinemachine;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera topCamera, ballCamera;

    public enum Camera
    {
        top,
        ball
    }

    public void SetCamera(Camera camera)
    {
        if(camera == Camera.ball)
        {
            ballCamera.Priority = topCamera.Priority + 1;
        }
        else
        {
            ballCamera.Priority = topCamera.Priority - 1;
        }
    }
}
