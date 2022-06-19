using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera topCamera, ballCamera;

    public enum Camera
    {
        top,
        ball
    }

    public float CameraBlendDuration
    {
        get => cinemachineBrain.m_DefaultBlend.BlendTime;
    }

    public WaitForSeconds WaitForCamera
    {
        get => new WaitForSeconds(CameraBlendDuration);
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

    public IEnumerator CameraToTargetCoroutine(CameraController.Camera target, WaitForSeconds waitForSeconds = null)
    {
        SetCamera(target);
        yield return waitForSeconds;
    }
}
