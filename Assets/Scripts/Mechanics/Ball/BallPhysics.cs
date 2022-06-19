using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    [SerializeField] private int maxTrajectoryBounce = 12;
    [SerializeField] private float shootStrength = 40f;
    [SerializeField] private float hitTargetAtY = 2f;
    [SerializeField] private float minPeakHeight = 0.1f;
    [SerializeField] private float maxPeakHeight = 8f;

    [SerializeField] private Ball ball;
    [SerializeField] private Rigidbody ballRigidbody;
    
    [SerializeField] private float gravity = -100f;

    private float Gravity
    {
        get => Physics.gravity.y;

        set
        {
            gravity = value;
            Physics.gravity = Vector3.up * gravity;
        }
    }

    private LayerMask _dummiesLayerMask;

    private void Awake()
    {
        _dummiesLayerMask = LayerMask.GetMask("Dummies");
    }

    private void OnValidate()
    {
        Gravity = gravity;
    }

    public List<Vector3> CalculateTrajectory(Vector3 aiming)
    {
        // Start trajectory from the standing position.
        var trajectoryPoints = new List<Vector3>();
        var trajectoryStartingPosition = transform.position;
        trajectoryPoints.Add(trajectoryStartingPosition);

        // Insert bounce off points.
        var motion = aiming * shootStrength;
        var bounceOffPoints = CalculateBounceOffPoints(ref motion);
        trajectoryPoints.AddRange(bounceOffPoints);

        // Add the last calculated reflection as where we left from.
        var ballStopPosition = trajectoryPoints.Last() + motion;
        trajectoryPoints.Add(ballStopPosition);

        return trajectoryPoints;
    }

    private List<Vector3> CalculateBounceOffPoints(ref Vector3 reflection)
    {
        var bounceOffPoints = new List<Vector3>();

        var limit = maxTrajectoryBounce;
        var shouldCheckBounce = limit > 0;

        RaycastHit hit;
        var ray = new Ray(transform.position, reflection);

        while (shouldCheckBounce)
        {
            var hits = Physics.SphereCast(ray, transform.localScale.x, out hit, reflection.magnitude, _dummiesLayerMask);

            if (hits)
            {
                bounceOffPoints.Add(hit.point);

                // Calculate reflection vector and update ray.
                reflection = BounceOffDummy(hit, reflection);
                ray.origin = hit.point;
                ray.direction = reflection;
            }

            limit--;
            shouldCheckBounce = hits && limit > 0;
        }

        return bounceOffPoints;
    }

    private Vector3 BounceOffDummy(RaycastHit hit, Vector3 reflection)
    {
        var dummy = hit.collider.gameObject.GetComponent<Dummy>();

        if (dummy != null)
        {
            return dummy.Bounce(reflection);
        }
        else
        {
            return reflection;
        }
    }

    public IEnumerator MoveBallCoroutine(Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            // Slow down the ball before it arrives to the last position.
            var isLastPoint = i == points.Length - 1;
            if (isLastPoint)
            {
                ballRigidbody.drag = 1f;
            }

            yield return MoveBallToPoint(points[i]);
        }
    }

    private IEnumerator MoveBallToPoint(Vector3 destination)
    {
        var origin = transform.position;

        var distanceX = destination.x - origin.x;
        var distanceY = destination.y - origin.y + hitTargetAtY;
        var distanceZ = destination.z - origin.z;
        var horizontalDistance = new Vector3(distanceX, 0, distanceZ);

        // Set the Peak Height to be higher further the destination is.
        var peakHeight = distanceY + peakHeightFromDistance(horizontalDistance.magnitude);

        var travelTime = Mathf.Sqrt(-2f * peakHeight / Gravity) + Mathf.Sqrt(2f * (distanceY - peakHeight) / Gravity);

        var horizontalVelocity = horizontalDistance / travelTime;
        var verticalVelocity = Vector3.up * Mathf.Sqrt(-2f * peakHeight * Gravity);

        ballRigidbody.velocity = verticalVelocity + horizontalVelocity;

        var waitForBallToMove = new WaitForSeconds(travelTime);
        yield return waitForBallToMove;
    }

    private float peakHeightFromDistance(float distance)
    {
        var distanceInRange = Mathf.InverseLerp(0f, shootStrength, distance);
        var distanceMapped = Mathf.Lerp(0f, 1f, distanceInRange);

        return Mathf.Lerp(minPeakHeight, maxPeakHeight, distanceMapped);
    }

    private void OnTriggerEnter(Collider collider)
    {
        CheckGoalPostHit(collider);
    }

    private void CheckGoalPostHit(Collider collider)
    {
        if (collider.CompareTag("GoalPost"))
        {
            ball.Score();
        }
    }

    public void ResetPhysics()
    {
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.drag = 0f;
    }
}
