using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Trajectory trajectory;
    [SerializeField] private BallPhysics ballPhysics;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private ParticleSystem scoreParticles;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float ballStopWaitTime;

    private Vector3 _cachedPosition;
    private bool _hasScored;

    public void Aim(Vector3 aiming)
    {
        trajectory.Reset();
        trajectory.Points = ballPhysics.CalculateTrajectory(aiming);
        trajectory.Draw();
    }

    public void Shoot()
    {
        TouchController.active = false;
        
        var points = trajectory.ProcessTrajectory();

        _cachedPosition = transform.position;

        var coroutine = ShootBallCoroutine(points);
        StartCoroutine(coroutine);
    }

    private IEnumerator ShootBallCoroutine(Vector3[] points)
    {   
        yield return cameraController.CameraToTargetCoroutine(CameraController.Camera.ball, cameraController.WaitForCamera);

        var moveBallCoroutine = ballPhysics.MoveBallCoroutine(points);
        yield return moveBallCoroutine;

        var waitForBallToStop = new WaitForSeconds(ballStopWaitTime);
        yield return waitForBallToStop;

        if(_hasScored)
        {
            LevelManager.LoadLevel(LevelManager.NextLevel);
        }

        yield return cameraController.CameraToTargetCoroutine(CameraController.Camera.top, cameraController.WaitForCamera);

        Reset();
    }

    public void Score()
    {
        scoreParticles.Play();

        _hasScored = true;
    }

    public void Reset()
    {
        TouchController.active = true;
        transform.SetPositionAndRotation(_cachedPosition, Quaternion.Euler(0, 0, 0));
        ballPhysics.ResetPhysics();
        trailRenderer.Clear();
    }
}
