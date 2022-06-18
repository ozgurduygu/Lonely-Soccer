using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] public BallPhysics ballPhysics;

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private ParticleSystem _scoreParticles;

    [SerializeField] private TrailRenderer _trailRenderer;

    public Trajectory trajectory;

    private Vector3 _cachedPosition;

    public void Aim(Vector3 strength)
    {
        trajectory.Reset();
        trajectory.Points = ballPhysics.CalculateTrajectory(strength);
        trajectory.Draw();
    }

    public void Shoot()
    {
        var points = trajectory.ProcessTrajectory();

        _cachedPosition = transform.position;

        var coroutine = ShootBallCoroutine(points);
        StartCoroutine(coroutine);
    }

    private IEnumerator ShootBallCoroutine(Vector3[] points)
    {
        TouchController.active = false;
        
        yield return CameraToBallCoroutine();

        var moveBallCoroutine = ballPhysics.MoveBallCoroutine(points);
        yield return moveBallCoroutine;

        yield return CameraToTopCoroutine();
        Reset();
        TouchController.active = true;
    }

    public void Score()
    {
        _scoreParticles.Play();
    }

    public void Reset()
    {
        transform.SetPositionAndRotation(_cachedPosition, Quaternion.Euler(0, 0, 0));
        ballPhysics.ResetPhysics();
        _trailRenderer.Clear();
    }

    private IEnumerator CameraToBallCoroutine()
    {
        _cameraController.SetCamera(CameraController.Camera.ball);
        yield return WaitForCamera();
    }

    private IEnumerator CameraToTopCoroutine()
    {
        _cameraController.SetCamera(CameraController.Camera.top);
        yield return WaitForCamera();
    }

    public WaitForSeconds WaitForCamera()
    {
        var duration = _cameraController.CameraBlendDuration;
        Debug.Log(duration);
        return new WaitForSeconds(duration);
    }
}
