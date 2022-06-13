using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 50f;
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float acceleration = 100f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float energyLoss = 5f;

    [SerializeField] private GameObject lineRenderer;
    private Trajectory trajectory;
    [SerializeField] private int maxIteration = 12;

    private Vector3 _spawnPosition;

    private void Awake()
    {
        _spawnPosition = transform.position;
        trajectory = lineRenderer.GetComponent<Trajectory>();
    }

    public Vector3[] CalculateTrajectory(Vector3 velocity)
    {
        trajectory.ClearPoints();

        var reflection = velocity;
        var collidersLayerMask = LayerMask.GetMask("Colliders");

        trajectory.AddPoint(transform.position);


        Ray ray = new Ray(transform.position, reflection);

        var i = 0;
        var doCheckHit = true;
        while (doCheckHit && i < maxIteration)
        {
            RaycastHit hit;

            doCheckHit = Physics.SphereCast(ray, transform.localScale.x, out hit, reflection.magnitude, collidersLayerMask);

            if (doCheckHit)
            {
                var dummy = hit.collider.gameObject.GetComponent<Dummy>();


                if (dummy)
                    reflection = dummy.Bounce(reflection);

                ray = new Ray(hit.point, reflection);

                trajectory.AddPoint(hit.point);
            }

            i++;
        }

        var lastPosition = trajectory.lastPoint + reflection;
        trajectory.AddPoint(lastPosition);

        trajectory.Draw();

        return new Vector3[] { };
    }

    public void EndTrajectory()
    {
    }

    public void Shoot()
    {
        // Attach camera to follow the ball
        Camera.main.GetComponent<CameraControls>().SetCamera(CameraControls.Camera.ball);
        var points = trajectory.ProcessTrajectory().ToArray();
        StartCoroutine(MoveBall(points));
    }

    private IEnumerator MoveBall(Vector3[] points)
    {
        Vector3 velocity = Vector3.zero;

        for (int i = 0; i < points.Length; i++)
        {
            var destination = points[i];

            var isFirstPoint = i == 0;
            var isLastPoint = i == points.Length - 1;

            var speed = maxSpeed;

            var distance2D = Distance2D(transform.position, destination);

            while (Vector3.Distance(transform.position, destination) >= maxSpeed * Time.deltaTime)
            {
                var interpolation = Vector2.zero;
                var progress = (distance2D - Vector3.Distance(transform.position, destination)) / distance;
                if (isFirstPoint)
                {
                    // interpolate horizontal position
                    interpolation.x = 0f;
                    // interpolate vertical position
                    interpolation.y = 0f;
                }
                else if (isLastPoint)
                {
                    // interpolate horizontal position
                    interpolation.x = 0f;
                    // interpolate vertical position
                    interpolation.y = 0f;
                    speed = maxSpeed - maxSpeed * progress;
                    Debug.Log(progress);
                }
                else
                {
                    // interpolate horizontal position
                    interpolation.x = 0f;
                    // interpolate vertical position
                    interpolation.y = 0f;
                }

                var newPosition = Vector3.MoveTowards(transform.position, destination, speed);

                transform.position = newPosition;

                yield return null;
            }
        }

        // Shoot finished, return camera and ball!
        yield return new WaitForSeconds(5);

        Camera.main.GetComponent<CameraControls>().SetCamera(CameraControls.Camera.top);
        transform.position = _spawnPosition;
    }

    private float Distance2D(Vector3 origin, Vector3 destination)
    {
        var origin2D = new Vector2(origin.x, origin.z);
        var destination2D = new Vector2(destination.x, destination.z);

        return Vector2.Distance(origin2D, destination2D);
    }
}
