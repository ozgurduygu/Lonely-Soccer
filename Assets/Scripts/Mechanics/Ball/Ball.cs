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
        // Disable input for the duration of shoot.
        TouchController.active = false;

        _cachedPosition = transform.position;

        var points = trajectory.ProcessTrajectory();
        var coroutine = ShootBallCoroutine(points);
        StartCoroutine(coroutine);
    }

    private IEnumerator ShootBallCoroutine(Vector3[] points)
    {
        var topCam = CameraController.Camera.top;
        var ballCam = CameraController.Camera.ball;
        var waitForCam = cameraController.WaitForCamera;

        // Wait for Camera to switch to ball.
        yield return cameraController.CameraToTargetCoroutine(ballCam, waitForCam);

        // Shoot the ball.
        var moveBallCoroutine = ballPhysics.MoveBallCoroutine(points);
        yield return moveBallCoroutine;

        // Wait a little after the move ends.
        var waitForBallToStop = new WaitForSeconds(ballStopWaitTime);
        yield return waitForBallToStop;

        // Wait for Camera to switch back to top view.
        yield return cameraController.CameraToTargetCoroutine(topCam, waitForCam);

        FinishShoot();
    }

    public void Score()
    {
        scoreParticles.Play();
        _hasScored = true;
    }

    private void FinishShoot()
    {
        TouchController.active = true;

        if (_hasScored)
        {
            LevelManager.LoadLevel(LevelManager.NextLevel);
        }

        Reset();
    }

    private void Reset()
    {
        transform.SetPositionAndRotation(_cachedPosition, Quaternion.Euler(0f, 0f, 0f));
        ballPhysics.ResetPhysics();
        trailRenderer.Clear();
    }
}
